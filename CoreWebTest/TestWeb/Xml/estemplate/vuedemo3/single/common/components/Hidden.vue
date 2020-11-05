<script>
export default {
  name: 'TkHidden',
  render: function (createElement) {
    return createElement(
      this.tag, {
      'class': {
        'tk-hidden': true
      },
      attrs: {
        prop: this.name
      }
    }, this.hidden
    )
  },
  props: {
    name: {
      type: String,
      required: true
    },
    tag: {
      type: String,
      default: 'div'
    },
    value: {}
  },
  computed: {
    hidden: {
      get () {
        return this.value
      },
      set (val) {
        this.$emit('input', val)
      }
    }
  },
  mounted () {
    if (!this.value) this.$emit('input', this.value)
  }
}
</script>
<style lang="scss" scoped>
.tk-hidden {
  display: none;
}
</style>