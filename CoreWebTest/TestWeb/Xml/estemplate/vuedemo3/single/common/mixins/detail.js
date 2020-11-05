import { replaceUriSegment, getQueryStringArgs } from 'common/utils/utils'
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
      // 从表
      activeName: '', //tab
      query: {}// query参数
    }
  },
  computed,
  watch: {
    $route: {
      handler: function (val, oldVal) {
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
      // 基础
      this.url = this.$refs.current.getAttribute('url')
      this.query = this.$route ? this.$route.query : getQueryStringArgs(this.data.url)
      this.getData()
      // end 基础
      // 从表
      // tab
      const first = this.$refs.tabs && this.$refs.tabs.$children[1] ? this.$refs.tabs.$children[1] : null
      if (first) {
        this.activeName = first.name
        this.handleClick(first)
      }
      document.getElementsByName('subTable').forEach((e, i) => {
        const childName = e.getAttribute('tablename')
        childName && this.getSubData(childName, i, e)
      })
      // end 从表
    },
    // 基础主表数据
    getData () {
      this.$axios.get(this.url, {
        params: this.query
      }).then(res => {
        const mainKey = this.$config.tableName
        this.formData[mainKey] = res[mainKey] || [{}]
        this.loaded = true
      }).catch(() => { })
    },
    // 从表
    handleClick (e) { //tab点击
      const loaded = e.$el.getAttribute('loaded')
      if (loaded || !e.name) return
      let index = e.index ? Number(e.index) : 0
      index && this.$refs.tabs.$children.length - 1 > this.$config.subTableNames.length && --index
      this.getSubData(e.name, index, e.$el)

    },
    getSubData (childName, index, e) {
      const url = replaceUriSegment(this.url, `CDetailList${index || ''}`)
      this.$axios.get(url, { params: Object.assign({ "ChildName": childName, "Index": index }, this.query) }).then(res => {
        this.formData[childName] = res[childName] || []
        e && e.setAttribute('loaded', true)
      }).catch(() => { })
    },
    // end 从表
    // 树形操作
    edit () {
      this.$router.push({
        path: '/edit',
        query: this.query
      })
    },
    del () {
      const url = replaceUriSegment(this.url, `delete`)
      this.$axios.get(url, {
        params: this.query
      }).then(res => {
        this.$emit('deleted')
      }).catch(() => { })
    },
    addChildNode () {
      this.$emit('append-child', this.query)
    }
  }
}
