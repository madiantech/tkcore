import dialogWrapper from '@/components/dialog-wrapper.vue'
import drawerWrapper from '@/components/drawer-wrapper.vue'

import request from '@/tkcore/utils/request'
import helpers from '@/utils/helpers'
import TkDebug from '@/tkcore/utils/TkDebug'
import TkMessageBox from '@/tkcore/utils/TkMessageBox'
import TkParamParser from '@/tkcore/utils/TkParamParser'

const exceptKeys = ['URL', 'Info', 'QueryString']

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
      activeNames: [],
      // 页面设置
      settings: {
        modifyButtonVisible: true,
        backButtonVisible: true
      },

      dataLoading: true, // 数据加载中
      // 数据
      data: {},

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
      if (to.name === 'MultiDetail' && this.$route.query.id) { // 判断id是否有值
        // 调数据
        this.loadPage('detailForm')
      }
    }
  },
  mounted() {
    this.expandAllCollapse()
    this.loadPage('detailForm')
  },
  methods: {

    // 页面关闭
    handleDialogClosed() {
      this.dialogVisible = false
    },
    handleDrawerClosed() {
      this.drawerVisible = false
    },

    handleBack() {
      if (this.isComponentModel) {
        this.$emit('form-closed')
      } else {
        this.$router.go(-1)
      }
    },
    // url解析
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
    // 数据获取
    loadPage(formName) {
      const url = this.$refs[formName].getAttribute('data-url')
      if (!url) {
        TkDebug.debug('toolkit/multi-edit.vue', '"data-url”属性未设置', false)
      }

      const path = this.parseUrl(url)
      // console.log(path)
      this.dataLoading = true
      request
        .get(path)
        .then(response => {
          // console.log(response)

          const arr = []
          for (const p in response) {
            if (exceptKeys.indexOf(p) === -1) {
              arr[p] = response[p]
            }
          }
          this.data = { ...arr }
        })
        .catch(error => {
          TkMessageBox.error('数据提交失败！请稍候重试。')
          TkDebug.debug('toolkit/multi-edit.vue', error)
        })
        .then(() => {
          this.dataLoading = false
        })
    },

    // 展开所有折叠
    expandAllCollapse() {
      if (this.activeNames.length === 0) {
        if (this.$refs.collapse) {
          for (const p in this.$refs.collapse.$children) {
            const collapse = this.$refs.collapse.$children[p].$attrs['collapse']
            if (collapse !== undefined && collapse === 'true') {
              this.activeNames.push(this.$refs.collapse.$children[p].name)
            }
          }
          // console.log(this.activeNames)
        }
      }
    }
  }
}
