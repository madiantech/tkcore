import Vue from 'vue'

Vue.mixin({
  methods: {
    callJsFunc(functionName) {
      // alert('convertToContext')
      if (typeof functionName === 'function') {
        functionName()
      } else if (typeof functionName === 'string') {
        // eslint-disable-next-line
        eval(functionName + '()')
      }
    },
    convertToContext(functionName) {
      this.callJsFunc(functionName)
    }
  }
})
