<script>
export default {
  name: 'TkTime',
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
      type: [String],
      default: ''
    },
    classname: {
      type: String
    },
    selectableRange: {
      type: [String, Array]
    },
    placeholder: {
      type: String,
      default: '请输入时间...'
    },
    format: {
      type: String,
      default: 'HH:mm:ss'
    },
    tooltip: {
      type: String
    },
    required: {
      type: Boolean
    },
    size: {
      type: String
    },
    labelWidth: {
      type: String
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
          'el-time-picker',
          {
            props: {
              value: this.value,
              placeholder: this.placeholder,
              readonly: this.readonly,
              'value-format': this.format,
              'picker-options': {
                selectableRange: this.selectableRange,
                format: this.format
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