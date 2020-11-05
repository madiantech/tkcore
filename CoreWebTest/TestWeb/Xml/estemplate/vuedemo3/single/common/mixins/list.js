export default {
  data () {
    return {
      tableData: [],
      RowOperator: [],
      Page: {
        TotalCount: 0,
        TotalPage: 0,
        CurrentPage: 0,
        PageSize: 15
      },
      Condition: {},
      SqlCon: '',
      bodyData: {},
      tableSortBackup: []
    }
  },
  computed: {
    sortOptions () {
      return this.tableSortBackup.length > 0 ? {
        JsonOrder: {
          "FieldList": this.tableSortBackup.filter(item => item.order).map(item => {
            return { "NickName": item.prop, "Order": item.order === 'ascending' ? 'Asc' : 'Desc' }
          })
        }
      } : {}
    }
  },
  mounted () {
    this.$on('ajax-success', () => {
      console.log('ajax-success')
      this.search()
    })
    this.$on('popupConfirm', () => {
      console.log('popupConfirm')
      this.search()
    })
    const tableName = Object.keys(this.$refs)[0]
    this.bodyData = {
      url: this.$refs[tableName].$attrs.url,
      tableName: tableName
    }
    this.search()
  },
  methods: {
    handleSizeChange () {
      // this.Page.CurrentPage = 0
      this.search()
    },
    handleCurrentChange (val) {
      this.Page.CurrentPage = val - 1
      this.search()
    },
    handleTheadAddClass ({ row, column, rowIndex, columnIndex }) {
      // 判断找到当前列 添加样式    
      const obj = this.tableSortBackup.find(item => item.prop === column.property)
      obj && (column.order = obj.order)
    },
    sortChange ({ column, prop, order }) {
      const obj = this.tableSortBackup.find(item => item.prop === prop)
      obj ? (obj.order = order) : this.tableSortBackup.push({ prop, order })
      this.search()
    },
    reSearch () {
      this.Page.CurrentPage = 0
      this.Page.TotalCount = 0
      this.search('post')
    },
    search (method = 'get') {
      let config = {
        method,
        url: this.bodyData.url,
        params: Object.assign({
          Page: this.Page.CurrentPage,
          PageSize: this.Page.PageSize,
          TotalCount: this.Page.TotalCount
        }, this.sortOptions)
      }
      if (method === 'post') {
        config.data = {
          Condition: this.Condition,
          IsEqual: false
        }
      } else if (this.SqlCon) {
        config.params.Condition = this.SqlCon
      }
      this.$axios(config).then(res => {
        this.tableData = res[this.bodyData.tableName]
        // 列表按钮
        this.RowOperator = res.RowOperator
        // 分页数据
        const countObj = res.Count[0] || {}
        Object.keys(countObj).forEach(key => {
          this.Page[key] = Number(countObj[key])
        })
        this.SqlCon = res.Sort[0].SqlCon || ''
      }).catch(() => { })
    },
    buttonShow (type, permision) {
      return permision ? permision.indexOf(type) > -1 : true
    },
    editConfirm (data) {
      const obj = data.data
      let item = this.tableData.find(ele => ele.Id === obj.Id)
      item && Object.assign(item, obj)
    },
    addConfirm (obj) {
      this.tableData.push(Object.assign({}, obj, { Id: this.tableData[this.tableData.length - 1].Id + 1 }))
    }
  }
}