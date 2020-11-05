<script>
export default {
  name: 'TkPassword',
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
    classname: {
      type: String
    },
    value: {
      type: [String, Number],
      default: ''
    },
    maxLength: {
      type: Number
    },
    minLength: {
      type: Number
    },
    readonly: {
      type: Boolean
    },
    showicon: {
      type: Boolean
    },
    size: {
      type: String
    },
    tooltip: {
      type: String
    },
    required: {
      type: Boolean
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
          'el-input',
          {
            props: {
              type: 'password',
              value: this.value,
              readonly: this.readonly,
              'show-password': this.showicon,
              size: this.size
            },
            attrs: {
              placeholder: this.placeholder,
              maxlength: this.maxLength,
              minlength: this.minLength
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