import dialogWrapper from '@/components/dialog-wrapper.vue'
import drawerWrapper from '@/components/drawer-wrapper.vue'

import request from '@/tkcore/utils/request'
import helpers from '@/utils/helpers'
import TkDebug from '@/tkcore/utils/TkDebug'
import TkMessageBox from '@/tkcore/utils/TkMessageBox'
import TkParamParser from '@/tkcore/utils/TkParamParser'

export default {
  components: {
    dialogWrapper,
    drawerWrapper
  },
  props: {
    // 是否显示
    isComponentModel: {
      type: Boolean,
      default: false
    },
    url: {
      type: String,
      default: ''
    }
  },
  data() {
    return {
      // 页面设置
      settings: {
        modifyButtonVisible: true,
        backButtonVisible: true
      },
      dataLoading: true, // 数据加载中
      // 数据
      data: {
        Name: '',
        Description: ''
      },

      // dialog 新增 or 修改
      dialogUrl: '',
      dialogTitle: '',
      dialogVisible: false,
      dialogWidth: '30%',
      dialogHeight: 0,

      // drawer新增 or 修改
      drawerUrl: '',
      drawerTitle: '',
      drawerVisible: false,
      drawerWidth: '30%'
    }
  },
  watch: {
    url: function() { // 组件模式下url变化时 刷新数据
      this.loadPage('detailForm')
    },
    '$route'(to, from) { // 非组件模式下，route变化是 刷新数据
      if (to.name === 'Detail' && this.$route.query.id) { // 判断id是否有值
        // 调数据
        this.loadPage('detailForm')
      }
    }
  },
  mounted() {
    this.loadPage('detailForm')
  },
  methods: {
    handleBack() {
      if (this.isComponentModel) {
        this.$emit('form-closed')
      } else {
        this.$router.go(-1)
      }
    },

    // 页面关闭
    handleDialogClosed() {
      this.dialogVisible = false
    },
    handleDrawerClosed() {
      this.drawerVisible = false
    },
    parseUrl(url) {
      let parseSource = null
      if (this.isComponentModel) {
        if (this.url) {
          parseSource = helpers.getQueryStrings(this.url)
        }
      } else {
        parseSource = this.$route.query
      }
      return TkParamParser.parseWithQueryStrings(parseSource, url)
    },
    loadPage(formName) {
      const url = this.$refs[formName].getAttribute('data-url')
      if (!url) {
        TkDebug.debug('toolkit/detail.vue', '"data-url”属性未设置', false)
      }
      const tableName = this.$refs[formName].getAttribute('data-table-name')
      if (!tableName) {
        TkDebug.debug('toolkit/detail.vue', '"data-table-name”属性未设置', false)
      }

      const path = this.parseUrl(url)
      // console.log(path)
      this.dataLoading = true
      request
        .get(path)
        .then(response => {
          // console.log(response)
          if (response[tableName] && response[tableName].length === 1) {
            this.data = response[tableName][0]
          } else {
            TkDebug.debug('toolkit/detail.vue', '请求返回值不可解析', false)
          }
        })
        .catch(error => {
          TkMessageBox.error('数据获取失败！请稍候重试。')
          TkDebug.debug('toolkit/detail.vue', error)
        })
        .then(() => {
          this.dataLoading = false
        })
    }

  }
}
