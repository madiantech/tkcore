import request from '@/tkcore/utils/request'
import helpers from '@/utils/helpers'
import utils from '@/tkcore/utils/utils'
import TkDebug from '@/tkcore/utils/TkDebug'
import TkMessageBox from '@/tkcore/utils/TkMessageBox'
import TkParamParser from '@/tkcore/utils/TkParamParser'

export default {
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
        resetButtonVisible: false,
        backButtonVisible: true,
        backButtonCaption: '返回'
      },
      dataLoading: true, // 数据加载中
      activeNames: [],
      hasGroup: false,
      // 数据
      data: {
      },
      // 后台验证错误时设置
      errors: {},
      // 验证规则（toolkit是否不需要这块）
      rules: {
        Name: [
          {
            required: true,
            message: 'please input Name',
            trigger: 'blur'
          }]
      }
    }
  },
  watch: {
    url: function() { // 组件模式下url变化时 刷新数据
      this.loadPage('editForm')
    },
    '$route'(to, from) { // 非组件模式下，route变化是 刷新数据
      if (to.name === 'Edit' && this.$route.query.id) { // 判断id是否有值
        // 调数据
        this.loadPage('editForm')
      }
    }
  },
  mounted() {
    this.settings.backButtonCaption = this.isComponentModel ? '关闭' : '返回'
    setTimeout(() => {
      this.loadPage('editForm')
    }, 100)
  },
  methods: {
    handleSubmitForm(formName) {
      this.errors = {}
      this.$refs[formName].validate((valid) => {
        if (valid) {
          const url = this.$refs[formName].$attrs['data-url']
          if (!url) {
            TkDebug.debug('toolkit/list.vue', '"data-url”属性未设置', false)
          }
          const tableName = this.$refs[formName].$attrs['data-table-name']
          if (!tableName) {
            TkDebug.debug('toolkit/list.vue', '"data-table-name”属性未设置', false)
          }

          const data = {}
          data[tableName] = []
          data[tableName].push(this.data)
          const path = this.parseUrl(url)
          this.dataLoading = true
          request
            .post(path, data, {})
            .then(response => {
              // console.log(response)
              if (response.Result) {
                if (response.Result.Result === 'Error') {
                  response.FieldInfo.forEach((item) => {
                    // console.log(item)
                    this.$set(this.errors, item.NickName, item.Message)
                    // this.errors[item.NickName] = item.Message
                  })
                  return
                } else if (response.Result.Result === 'Success') {
                  switch (response.Result.Message) {
                    case 'WeixinClose': // 在微信中关闭窗口
                      // TODO:WeixinClose
                      TkMessageBox.error('WeixinClose暂未实现，联系开发者实现')
                      break
                    case 'Refresh': // 刷新当前页面
                      this.loadPage('editForm')
                      break
                    case 'Back': // 返回上一页等同于浏览器的back按钮
                      this.handleBack()
                      break
                    case 'CloseDialog': // 关闭对话框
                      if (this.isComponentModel) {
                        this.$emit('form-closed', 'CloseDialog')
                      }
                      break
                    case 'ListRefresh': // 刷新列表页面（和刷新不一样，会考虑列表分页，查询条件等因素）
                      TkMessageBox.error('ListRefresh暂未实现，联系开发者实现')
                      break
                    case 'CloseDialogAndRefresh': // 关闭对话框并使用列表刷新的方式刷新页面
                      if (this.isComponentModel) {
                        this.$emit('form-closed', 'CloseDialogAndRefresh')
                      }
                      break
                    default: // 	如无法识别，认为它是Url，进行跳转
                      utils.JumpUrl(this, response.Result.Message, '', response.NewWindow, false, null)
                      break
                  }
                }
              } else {
                TkMessageBox.error('返回的数据格式无法解析')
                TkDebug.debug('toolkit/edit.vue', response)
              }
            })
            .catch(error => {
              TkMessageBox.error('数据提交失败！请稍候重试。')
              TkDebug.debug('toolkit/edit.vue', error)
            })
            .then(() => {
              this.dataLoading = false
            })
        } else {
          return false
        }
      })
    },
    handleReset(formName) {
      this.errors = {}
      this.$refs[formName].resetFields()
      this.loadPage('editForm')
    },
    handleBack() {
      if (this.isComponentModel) {
        this.$emit('form-closed')
      } else {
        this.$router.go(-1)
      }
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
      const url = this.$refs[formName].$attrs['data-url']
      if (!url) {
        TkDebug.debug('toolkit/list.vue', '"data-url”属性未设置', false)
      }
      const tableName = this.$refs[formName].$attrs['data-table-name']
      if (!tableName) {
        TkDebug.debug('toolkit/list.vue', '"data-table”属性未设置', false)
      }
      if (!tableName) {
        TkDebug.debug('toolkit/list.vue', '"data-table”属性未设置', false)
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
            TkDebug.debug('toolkit/list.vue', '请求返回值不可解析', false)
          }
        })
        .catch(error => {
          TkMessageBox.error('数据提交失败！请稍候重试。')
          TkDebug.debug('toolkit/list.vue', error)
        })
        .then(() => {
          this.dataLoading = false
          this.expandAllCollapse()
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
