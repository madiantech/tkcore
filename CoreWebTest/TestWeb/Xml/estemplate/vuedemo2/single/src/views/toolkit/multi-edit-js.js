import request from '@/tkcore/utils/request'
import helpers from '@/utils/helpers'
import utils from '@/tkcore/utils/utils'
import TkDebug from '@/tkcore/utils/TkDebug'
import TkMessageBox from '@/tkcore/utils/TkMessageBox'
import TkParamParser from '@/tkcore/utils/TkParamParser'

const exceptKeys = ['URL', 'Info', 'QueryString']

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
      activeNames: [],
      // 页面设置
      settings: {
        resetButtonVisible: false,
        backButtonVisible: true,
        backButtonCaption: '返回'
      },

      dataLoading: true, // 数据加载中
      // 数据
      data: {},
      // 后台验证错误时设置
      errors: {},
      rules: {}
    }
  },

  watch: {
    url: function() { // 组件模式下url变化时 刷新数据
      this.loadPage('editForm')
    },
    '$route'(to, from) { // 非组件模式下，route变化是 刷新数据
      if (to.name === 'MultiEdit' && this.$route.query.id) { // 判断id是否有值
        // 调数据
        this.loadPage('editForm')
      }
    }
  },
  mounted() {
    this.settings.backButtonCaption = this.isComponentModel ? '关闭' : '返回'
    this.expandAllCollapse()
    this.loadPage('editForm')
  },
  methods: {
    // 全选
    handleCheckAll(tableName) {
      this.$refs[tableName].clearSelection()
      this.$refs[tableName].toggleAllSelection()
    },
    // 反选
    handleCheckReverse(tableName) {
      for (const row in this.data[tableName]) {
        this.$refs[tableName].toggleRowSelection(this.data[tableName][row])
      }
    },
    // 全不选
    handleUncheckAll(tableName) {
      this.$refs[tableName].clearSelection()
    },
    // 删除行
    handleDeleteRow(tableName) {
      const selection = this.$refs[tableName].selection

      if (selection.length === 0) {
        TkMessageBox.info('请选择要删除的数据行')
        return
      }

      TkMessageBox.confirm('确定删除所选行的数据行？').then(() => {
        let isEqual = false

        for (const row2 in selection) {
          for (const row in this.data[tableName]) {
            console.log(isEqual)
            isEqual = this.data[tableName][row] === selection[row2]
            if (isEqual) {
              delete this.data[tableName][row]
              break
            }
          }
        }
        this.$refs[tableName].clearSelection()
      })
    },
    // 全删
    handleDeleteAll(tableName) {
      TkMessageBox.confirm('确认删除全部数据行？').then(() => {
        this.data[tableName] = []
        this.$refs[tableName].clearSelection()
      })
    },
    // 新建
    handleCommand(tableName, command) {
      if (command === 'custom') {
        TkMessageBox.prompt('请输入新建行数(1-99)？', '确认', {
          inputPattern: /^[0-9]{1,2}$/,
          inputErrorMessage: '数字格式不正确'
        }).then((res) => {
          const lines = isNaN(parseInt(res.value)) ? 1 : parseInt(res.value)
          if (lines === 0) {
            TkMessageBox.info('行数必须大于1')
            return
          }
          this.createNewLine(tableName, lines)
        })
      } else {
        this.createNewLine(tableName, parseInt(command))
      }
    },
    createNewLine(tableName, lines) {
      // console.log(lines)
      const templates = JSON.parse(this.$refs.newLineTemplates.innerHTML)
      for (let i = 1; i <= lines; i++) {
        this.data[tableName].push(Object.assign({}, templates[tableName]))
      }
    },

    // 全局按钮
    async handleSubmitForm(formName) {
      this.initErrors()
      for (const p in this.$refs) {
        if (p.endsWith('SubForm')) {
          const res = await this.validateSync(p)
          if (!res) {
            return false
          }
        }
      }

      const url = this.$refs[formName].getAttribute('data-url')
      if (!url) {
        TkDebug.debug('toolkit/multi-edit.vue', '"data-url”属性未设置', false)
      }

      const data = {}

      for (const p in this.data) {
        if (exceptKeys.indexOf(p) === -1) {
          data[p] = this.data[p]
        }
      }
      const path = this.parseUrl(url)
      // console.log(path)
      this.dataLoading = true
      request
        .post(path, data, {})
        .then(response => {
          // console.log(response)
          if (response.Result) {
            if (response.Result.Result === 'Error') {
              if (response.FieldInfo.length > 0) {
                const errors = {}

                for (let i = 0; i <= response.FieldInfo.length - 1; i++) {
                  const item = response.FieldInfo[i]
                  if (errors[item.TableName] === undefined) {
                    errors[item.TableName] = []
                  }
                  if (errors[item.TableName][item.Position] === undefined) {
                    errors[item.TableName][item.Position] = {}
                  }
                  errors[item.TableName][item.Position][item.NickName] = item.Message
                }
                this.errors = { ...errors }
              }
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
            TkDebug.debug('toolkit/multi-edit.vue', response)
          }
        })
        .catch(error => {
          TkMessageBox.error('数据提交失败！请稍候重试。')
          TkDebug.debug('toolkit/multi-edit.vue', error)
        })
        .then(() => {
          this.dataLoading = false
        })
    },
    handleReset(formName) {
      this.initErrors()
      for (const p in this.$refs) {
        if (p.endsWith('SubForm')) {
          this.$refs[p].resetFields()
        }
      }
      this.loadPage('editForm')
    },
    handleBack() {
      if (this.isComponentModel) {
        this.$emit('form-closed')
      } else {
        this.$router.go(-1)
      }
    },
    validateSync(formName) {
      return new Promise((resolve, reject) => {
        this.$refs[formName].validate((valid) => {
          resolve(valid)
        })
      })
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
          this.initErrors()
        })
        .catch(error => {
          TkMessageBox.error('数据提交失败！请稍候重试。')
          TkDebug.debug('toolkit/multi-edit.vue', error)
        })
        .then(() => {
          this.dataLoading = false
        })
    },
    initErrors() {
      this.errors = {}

      for (const p in this.data) {
        if (exceptKeys.indexOf(p) === -1) {
          this.errors[p] = []
          for (const r in this.data[p]) {
            this.errors[p].push({})
          }
        }
      }
    },
    // 展开所有折叠
    expandAllCollapse() {
      // if (this.activeNames.length === 0) {
      //   const templates = JSON.parse(this.$refs.newLineTemplates.innerHTML)
      //   for (const p in templates) {
      //     if (exceptKeys.indexOf(p) === -1) {
      //       this.activeNames.push(p)
      //     }
      //   }
      // }

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
    },
    // 获取错误信息 key: table.pos.field
    errorText(key) {
      const arr = key.split('.')
      if (arr.length !== 3) {
        return ''
      }
      if (!this.errors[arr[0]]) {
        return ''
      }

      if (!this.errors[arr[0]][arr[1]]) {
        return ''
      }
      return this.errors[arr[0]][arr[1]][arr[2]]
    }
  }
}
