export default {
  data () {
    return {
      loaded: false,
      url: ''
    }
  },
  mounted () {
    this.url = this.$refs.current.getAttribute('url')
    this.getData()
  },
  methods: {
    getData () {
      this.$axios.get(this.url).then(res => {
        const mainKey = this.config.tableName
        this.formData[mainKey] = res.data[mainKey] || [{}]
        this.loaded = true
      })
    },
    confirm () {
      return this.$axios.post(this.url, this.formData).then(res => {
        return Promise.resolve()
      }).catch(() => {
        return Promise.reject()
      })
    }
  }
}