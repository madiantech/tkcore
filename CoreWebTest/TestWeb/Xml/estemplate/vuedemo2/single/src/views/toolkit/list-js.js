
import request from '@/tkcore/utils/request'
import helpers from '@/utils/helpers'
import dialogWrapper from '@/components/dialog-wrapper.vue'
import drawerWrapper from '@/components/drawer-wrapper.vue'
import TkDebug from '@/tkcore/utils/TkDebug'
import TkMessageBox from '@/tkcore/utils/TkMessageBox'

const sortedCols = {}

export default {
  components: {
    dialogWrapper,
    drawerWrapper
  },
  data() {
    return {
      // 页面设置
      settings: {
        conditionButtonPosition: 'inline', // 查询条件的按钮位置 bottom or inline
        conditionResetButtonVisible: true, // 查询条件的重置按钮是否显示
        multiColSortable: true // true-支持多列排序 false-单列排序
      },
      activeTabName: '',
      // 条件
      condition: {
        Name: '',
        Description: ''
      },
      isEqual: false,

      // 数据
      dataset: [],
      dataLoading: true, // 数据加载中

      // 翻页
      pagination: {
        currentPage: 1, // 从1开始计算，传给后台前需要减1
        pageSizes: [15, 30, 45, 60],
        pageSize: 15,
        total: 0
      },

      // dialog 新增 or 修改
      dialogUrl: '',
      dialogTitle: '',
      dialogVisible: false,
      dialogWidth: '30%',
      dialogHeight: 0,
      dialogCloseOnClickModal: false,

      // drawer新增 or 修改
      drawerUrl: '',
      drawerTitle: '',
      drawerVisible: false,
      drawerWidth: '30%',
      drawerCloseOnClickModal: false,

      // 为了应对toolikit的机制预留的字段
      toolkit: {
        totalPage: 0,
        condition: '',
        JsonOrder: {
          FieldList: []
        }
      }

    }
  },
  mounted() {
    this.initByQueryStrings()
    this.loadPage()
  },
  methods: {
    // 根据Querystring初始化变量
    initByQueryStrings() {
      this.pagination.currentPage = this.$route.query.Page ? Number(this.$route.query.Page) + 1 : 1
      this.pagination.pageSize = this.$route.query.PageSize ? Number(this.$route.query.PageSize) : 15
      this.toolkit.condition = this.$route.query.Condition || ''
      this.activeTabName = this.$route.query.Tab || ''
      this.pagination.total = this.$route.query.TotalCount ? Number(this.$route.query.TotalCount) : 0
      this.pagination.totalPage = this.$route.query.TotalPage ? Number(this.$route.query.TotalPage) : 0
      if (this.$route.query.JsonOrder) {
        this.setSortedCols(this.$route.query.JsonOrder)
      }
    },
    hasOperatorRight(row, operator) {
      // console.log(row, operator)
      const rights = row['_OPERATOR_RIGHT']
      if (!rights) {
        return false
      }
      return rights.indexOf(operator) >= 0
    },
    // condition
    handleTabClick(tab, event) {
      // console.log(tab, event)
      this.activeTabName = tab.name
      this.pagination.currentPage = 1
      this.pagination.total = 0
    },
    handleSearch() {
      this.pagination.currentPage = 1
      this.pagination.total = 0
      this.toolkit.totalPage = 0
      this.query()
    },
    handleReset(formName) {
      this.$refs[formName].resetFields()
    },

    // gloal-button

    // Table
    handleSortChange({ column, prop, order }) {
      // console.log(column, prop, order)
      if (column.property === null) {
        return
      }
      if (order === null) {
        sortedCols[column.property] = null
      } else {
        if (!this.settings.multiColSortable) {
          for (const key in sortedCols) {
            if (key !== column.property) {
              sortedCols[key] = null
            }
          }
        }
        if (!sortedCols[column.property]) {
          sortedCols[column.property] = order
        } else {
          sortedCols[column.property] = sortedCols[column.property] === 'descending' ? 'ascending' : 'descending'
        }
      }

      // console.log(sortedCols)
      this.loadPage()
    },
    handleTheadAddClass({ row, column, rowIndex, columnIndex }) {
      // console.log(column, columnIndex)

      if (column.property === null) {
        return ''
      }
      const sort = sortedCols[column.property]
      if (!sort) {
        return ''
      } else {
        // console.log(column, columnIndex, 'active-thead ' + sort)
        return 'active-thead ' + sort
      }
    },
    // pagination
    handleSizeChange(val) {
      console.log(`每页 ${val} 条`)
      this.pagination.pageSize = val
      this.loadPage()
    },
    handleCurrentChange(val) {
      console.log(`当前页: ${val}`)
      this.pagination.currentPage = val
      this.loadPage()
    },
    // 页面关闭
    handleDialogClosed(e) {
      this.dialogVisible = false
      if (e === 'CloseDialogAndRefresh') {
        this.loadPage()
      }
    },
    handleDrawerClosed(e) {
      this.drawerVisible = false
      if (e === 'CloseDialogAndRefresh') {
        this.loadPage()
      }
    },

    // 加载页面数据
    loadPage() {
      const url = this.$refs.table.$attrs['data-url']
      if (!url) {
        TkDebug.debug('toolkit/list.vue', '"data-url”属性未设置', false)
      }
      const tableName = this.$refs.table.$attrs['data-table-name']
      if (!tableName) {
        TkDebug.debug('toolkit/list.vue', '"data-table”属性未设置', false)
      }

      const jsonOrder = {
        FieldList: []
      }
      for (const key in sortedCols) {
        if (sortedCols[key]) {
          jsonOrder.FieldList.push({
            NickName: key,
            Order: sortedCols[key] === 'descending' ? 'DESC' : 'ASC'
          })
        }
      }

      // data
      const data = {
        Page: this.pagination.currentPage - 1,
        PageSize: this.pagination.pageSize,
        Condition: this.toolkit.condition || '',
        Tab: this.activeTabName,
        TotalCount: this.pagination.total || 0,
        TotalPage: this.toolkit.totalPage || 0,
        JsonOrder: JSON.stringify(jsonOrder)
      }

      const param = {}

      helpers.extend(param, data)
      // console.log(param)
      const path = helpers.addQueryString(url, param)
      // console.log(path)
      this.dataLoading = true
      request
        .get(path)
        .then(response => {
          // console.log(response)
          if (response.Count.length > 0) {
            this.pagination.total = Number(response.Count[0].TotalCount)
            this.pagination.pageSize = Number(response.Count[0].PageSize)
            this.toolkit.totalPage = Number(response.Count[0].TotalPage)
          }
          if (response.Sort.length > 0) {
            this.toolkit.condition = response.Sort[0].SqlCon
            this.toolkit.JsonOrder = response.Sort[0].JsonOrder
            this.setSortedCols(response.Sort[0].JsonOrder)
          }

          this.dataset = response[tableName]
        })
        .catch(error => {
          TkMessageBox.error('数据提交失败！请稍候重试。')
          TkDebug.debug('toolkit/list.vue', error)
        })
        .then(() => {
          this.dataLoading = false
        })
    },

    // 查询数据
    query() {
      const url = this.$refs.table.$attrs['data-url']
      if (!url) {
        TkDebug.debug('toolkit/list.vue', '"data-url”属性未设置', false)
      }
      const tableName = this.$refs.table.$attrs['data-table-name']
      if (!tableName) {
        TkDebug.debug('toolkit/list.vue', '"data-table”属性未设置', false)
      }

      const jsonOrder = {
        FieldList: []
      }
      for (const key in sortedCols) {
        if (sortedCols[key]) {
          jsonOrder.FieldList.push({
            NickName: key,
            Order: sortedCols[key] === 'descending' ? 'DESC' : 'ASC'
          })
        }
      }

      // data
      const data = {
        Page: this.pagination.currentPage - 1,
        PageSize: this.pagination.pageSize,
        Condition: this.toolkit.condition || '',
        Tab: this.activeTabName,
        TotalCount: this.pagination.total || 0,
        TotalPage: this.toolkit.totalPage || 0,
        JsonOrder: JSON.stringify(jsonOrder)
      }

      const postData = {
        Condition: this.condition,
        IsEqual: this.isEqual
      }
      const path = helpers.addQueryString(url, data)
      console.log(path)
      console.log(postData)
      this.dataLoading = true
      request
        .post(path, postData)
        .then(response => {
          console.log(response)
          // TODO:错误时返回结果如下，暂未处理 {Result:{Message:'',Result:''}}
          if (response.Count.length > 0) {
            this.pagination.total = Number(response.Count[0].TotalCount)
            this.pagination.pageSize = Number(response.Count[0].PageSize)
            this.toolkit.totalPage = Number(response.Count[0].TotalPage)
          }
          if (response.Sort.length > 0) {
            this.toolkit.condition = response.Sort[0].SqlCon
            this.toolkit.JsonOrder = response.Sort[0].JsonOrder
            this.setSortedCols(response.Sort[0].JsonOrder)
          }
          this.dataset = response[tableName]
        })
        .catch(error => {
          TkMessageBox.error('数据提交失败！请稍候重试。')
          TkDebug.debug('toolkit/list.vue', error)
        })
        .then(() => {
          this.dataLoading = false
        })
    },

    // 根据jsonOrder设置排序字段的状态
    setSortedCols(jsonOrder) {
      // console.log('sorting 1:', sortedCols)
      jsonOrder = JSON.parse(jsonOrder)
      if (!jsonOrder) return
      if (!jsonOrder.FieldList || jsonOrder.FieldList.length === 0) return
      let sortItem = null
      for (let i = 0; i <= jsonOrder.FieldList.length - 1; i++) {
        sortItem = jsonOrder.FieldList[i]
        sortedCols[sortItem.NickName] = sortItem.Order === 'DESC' ? 'descending' : 'ascending'
      }

      // console.log('sorting 2:', sortedCols)
    }
  }

}
