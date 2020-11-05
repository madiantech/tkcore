export default {
  props: {
    notform: {
      type: Boolean,
      default: false
    },
    name: {
      type: String,
      required: true
    },
    caption: {
      type: String
    },
    readonly: {
      type: Boolean
    },
    value: {
      type: [String, Date],
      default: ''
    },
    classname: {
      type: String
    },
    placeholder: {
      type: String,
      default: '选择日期...'
    },
    defaultValue: {},
    format: {
      type: String,
      default: 'yyyy-MM-dd'
    },
    tooltip: {
      type: String
    },
    required: {
      type: Boolean
    },
    min: {
      type: [String, Date],
      default: '1800-01-01'
    },
    max: {
      type: [String, Date],
      default: '3000-01-01'
    },
    size: {
      type: String
    },
    labelWidth: {
      type: String
    }
  },
  methods: {
    disabledDate (time) {
      let minDate, maxDate
      if (this.min instanceof Date) {
        minDate = this.min
      } else {
        const minArr = this.min.split(' ')
        minDate = new Date(`${this.min}${minArr.length > 1 ? '' : ' 00:00:00'}`)
      }
      if (this.max instanceof Date) {
        maxDate = this.max
      } else {
        const maxArr = this.max.split(' ')
        maxDate = new Date(`${this.max}${maxArr.length > 1 ? '' : ' 23:59:59'}`)
      }
      return time.getTime() < minDate.getTime() || time.getTime() > maxDate.getTime()
    }
  }
}