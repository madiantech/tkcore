<script>
import mixin from './mixin'
export default {
  name: 'TkDateRange',
  mixins: [mixin],
  props: {
    type: {
      tyep: String,
      default: 'daterange'
    },
    rangeSeparator: {
      type: String,
      default: '-'
    },
    endValue: {
      type: [String, Date],
      default: ''
    }
  },
  render (createElement) {
    return createElement(
      this.notform ? 'div' : 'el-form-item',
      {
        class: this.classname,
        props: {
          prop: this.name,
          label: this.caption,
          'label-width': this.labelWidth,
          required: this.required,
          size: this.size
        },
        directives: [
          {
            name: 'tk-help',
            value: this.tooltip
          }
        ]
      },
      [
        createElement(
          'el-date-picker',
          {
            props: {
              type: this.type,
              value: this.input,
              'start-placeholder': this.placeholder,
              'end-placeholder': this.placeholder,
              readonly: this.readonly,
              'default-value': this.defaultValue,
              'value-format': this.format,
              format: this.format,
              'picker-options': {
                disabledDate: this.disabledDate
              },
              'range-separator': this.rangeSeparator
            },
            on: {
              input: val => {
                this.input = val
              }
            }
          }
        )
      ]
    )
  },
  computed: {
    input: {
      get () {
        return [this.value, this.endValue]
      },
      set (val) {
        this.$emit('input', val && val[0] ? val[0] : '')
        this.$emit('update:endValue', val && val[1] ? val[1] : '')
      }
    }
  }
}
</script>
