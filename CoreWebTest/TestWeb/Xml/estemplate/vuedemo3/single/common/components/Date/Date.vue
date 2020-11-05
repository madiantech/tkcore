<script>
import mixin from './mixin'
export default {
  name: 'TkDate',
  mixins: [mixin],
  props: {
    type: {
      tyep: String,
      default: 'date'
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
              value: this.value,
              placeholder: this.placeholder,
              readonly: this.readonly,
              'default-value': this.defaultValue,
              'value-format': this.format,
              format: this.format,
              'picker-options': {
                disabledDate: this.disabledDate
              }
            },
            on: {
              input: val => {
                this.$emit('input', val)
              }
            }
          }
        )
      ]
    )
  }
}
</script>