<script>
export default {
  name: 'TkCheckbox',
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
    labelWidth: {
      type: String
    },
    classname: {
      type: String
    },
    value: {
      type: [String, Number]
    },
    readonly: {
      type: Boolean
    },
    onValue: {
      type: String,
      default: '1'
    },
    offValue: {
      type: String,
      default: '0'
    },
    tooltip: {
      type: String
    },
    required: {
      type: Boolean
    },
    size: {
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
          'el-checkbox',
          {
            props: {
              value: this.value,
              'true-label': this.onValue,
              'false-label': this.offValue,
              disabled: this.readonly,
              size: this.size
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