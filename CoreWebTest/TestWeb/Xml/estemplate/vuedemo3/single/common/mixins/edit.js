import { deepClearObject, isEmptyObject, getQueryStringArgs } from 'common/utils/utils'
import { formData, tableTemplete, computed } from 'common/utils/mixins'
export default {
  data () {
    return {
      // 基础
      data: {
        url: this.$route ? this.$route.fullPath : ''
      },
      formData,
      tableTemplete,
      loaded: false,
      url: '',
      // end 基础
      // 多表内容
      currentRow: {},
      multipleSelection: {},
      delData: {},
      addIndex: {},
      // end 多表内容
      isInitiativeLeave: false,
      query: {}// query参数
    }
  },
  computed,
  watch: {
    $route: {
      handler: function (val, oldVal) {
        console.log(val)
        this.data.url = val.fullPath
        this.start()
      },
      // 深度观察监听
      deep: true
    }
  },
  mounted () {
    this.start()
  },
  methods: {
    start () {
      this.url = this.$refs.current.getAttribute('url')
      this.query = this.$route ? this.$route.query : getQueryStringArgs(this.data.url)
      this.getData()
    },
    // 基础
    getData () {
      this.$axios.get(this.url, {
        params: this.query
      }).then(res => {
        // 主表
        const mainKey = this.$config.tableName
        this.formData[mainKey] = res[mainKey] || [{}]
        if (isEmptyObject(this.tableTemplete[mainKey])) {
          this.tableTemplete[mainKey] = deepClearObject(this.formData[mainKey][0] || {})
        }
        // 从表
        const subTableNames = this.$config.subTableNames || []
        subTableNames.forEach(key => {
          this.formData[key] = res[key] || []
          if (isEmptyObject(this.tableTemplete[key])) {
            this.tableTemplete[key] = deepClearObject(this.formData[key][0] || {})
          }
        })
        this.loaded = true
      }).catch(() => { })
    },
    confirm () {
      const subTableNames = this.$config.subTableNames || []
      subTableNames.forEach(key => {
        this.formData[key].forEach(obj => {
          obj._Status = obj.Id ? 'changed' : 'Insert'
        })
        this.formData[key].push.apply(this.formData[key], this.delData[key] || [])
      })
      return this.$axios.post(this.url, this.formData, {
        params: this.query
      }).then(res => {
        this.isInitiativeLeave = true
        this.$emit('confirm', res)
        return Promise.resolve()
      }).catch(err => {
        this.$message.error(String(err))
        return Promise.reject()
      })
    },
    cancel () {
      this.isInitiativeLeave = true
      this.$emit('cancel')
    },
    // end 基础
    // 多表内容
    handleCurrentChange (tablename, currentRow, oldCurrentRow) {
      this.currentRow[tablename] = currentRow
    },
    handleSelectionChange (tablename, val) {
      this.multipleSelection[tablename] = val
    },
    handleAdd (tablename) {
      this.addIndex[tablename] ? ++this.addIndex[tablename] : (this.addIndex[tablename] = 1)
      this.formData[tablename].push(Object.assign({ AddIndex: this.addIndex[tablename] }, this.tableTemplete[tablename]))
    },
    handleClear (tablename) {
      this.formData[tablename] = []
      this.multipleSelection[tablename] = []
      this.delData[tablename] = []
      this.addIndex[tablename] = 1
    },
    handleDelete (tablename) {
      const data = this.multipleSelection[tablename] || []
      data.forEach(item => {
        let index = -1
        if (item.Id) {
          index = this.formData[tablename].findIndex(obj => obj.Id === item.Id)
          !this.delData[tablename] && (this.delData[tablename] = [])
          this.delData[tablename].push(Object.assign({}, this.formData[tablename][index], { _Status: 'Delete' }))
        } else if (item.AddIndex) {
          index = this.formData[tablename].findIndex(obj => obj.AddIndex === item.AddIndex)
        }
        index > -1 && this.formData[tablename].splice(index, 1)
      })
      this.$refs[tablename].clearSelection()
    },
    handleSelectAll (tablename) {
      const selected = this.multipleSelection[tablename] || []
      selected.length < this.formData[tablename].length && this.$refs[tablename].toggleAllSelection()
    },
    handleSelectClear (tablename) {
      this.$refs[tablename].clearSelection()
    },
    handleSelectReverse (tablename) {
      this.formData[tablename].forEach(row => {
        this.$refs[tablename].toggleRowSelection(row)
      })
    },
    handleMove (tablename, direction) {
      const currentRow = this.currentRow[tablename]
      const index = this[tablename].findIndex(obj => obj.AddIndex ? obj.AddIndex === currentRow.AddIndex : obj.Id === currentRow.Id)
      switch (direction) {
        case 'Up':
          if (index > 0) {
            // [this[tablename][index], this[tablename][index - 1]] = [this[tablename][index - 1], this[tablename][index]]
            this[tablename].splice(index, 1, ...this[tablename].splice(index - 1, 1, this[tablename][index]))
            this.$forceUpdate()
          }
          break
        case 'Down':
          if (index < this[tablename].length - 1) {
            // [this[tablename][index], this[tablename][index + 1]] = [this[tablename][index + 1], this[tablename][index]]
            this[tablename].splice(index, 1, ...this[tablename].splice(index + 1, 1, this[tablename][index]))
            this.$forceUpdate()
          }
          break
      }
    }
    // end 多表内容
  },
  beforeRouteLeave (to, from, next) {
    if (!this.isInitiativeLeave) {
      this.$confirm('当前页面未保存，是否直接离开', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        this.$emit('cancel', true)
        next()
      }).catch(() => {
        this.$parent.$emit('reset-current-node')
      })
    } else {
      next()
    }
  }
}