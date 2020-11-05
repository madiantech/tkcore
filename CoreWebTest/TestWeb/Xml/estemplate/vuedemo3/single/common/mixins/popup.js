import Vue from 'vue'
export default {
  data () {
    return {
      visible: false,
      options: {
        title: '',
        width: '50%',
        cancel: null,
        confirm: null,
        fullscreen: false
      },
      component: null,
      current: null
    }
  },
  mounted () {
    this.visible = true
  },
  methods: {
    open () {
      if (this.component) {
        this.$nextTick(() => {
          const Component = Vue.extend(this.component)
          this.current = new Component({
            el: this.$refs.slot,
            data: {
              data: this.data
            }
          })
          // this.current.data = this.data
        })
      }
    },
    cancelFunc () {
      this.options.cancel && this.options.cancel(this.data)
      this.visible = false
    },
    confirmFunc () {
      this.options.confirm && this.options.confirm(this.data)
      if (this.current.confirm) {
        this.current.confirm().then(() => {
          this.$emit('confirm')
          this.visible = false
        }).catch(err => {
          console.log('submit error')
        })
      } else {
        this.$emit('confirm')
        this.visible = false
      }
    }
  }
}