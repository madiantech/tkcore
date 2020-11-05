<script>
export default {
  name: 'TkTextarea',
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
    placeholder: {
      type: String,
      default: '请输入...'
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
    autoSize: {
      type: [Boolean, Object]
    },
    rows: {
      type: Number
    },
    maxLength: {
      type: Number
    },
    minLength: {
      type: Number
    },
    disabled: {
      type: Boolean,
      default: false
    },
    readonly: {
      type: Boolean
    },
    size: {
      type: String
    },
    resize: {
      type: String
    },
    tooltip: {
      type: String
    },
    required: {
      type: Boolean
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
          'el-input',
          {
            props: {
              value: this.value,
              type: 'textarea',
              autosize: this.autoSize,
              disabled: this.disabled,
              readonly: this.readonly,
              resize: this.resize,
              size: this.size
            },
            attrs: {
              placeholder: this.placeholder,
              maxlength: this.maxLength,
              minlength: this.minLength,
              autofocus: this.autoFocus,
              rows: this.rows,
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