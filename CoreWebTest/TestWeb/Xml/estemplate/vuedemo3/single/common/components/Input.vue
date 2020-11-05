<script>
export default {
  name: 'TkInput',
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
    type: {
      type: String,
      default: 'text'
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
    icon: {
      type: String
    },
    readonly: {
      type: Boolean
    },
    autoFocus: {
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
              value: this.value,
              type: this.type,
              disabled: this.disabled,
              'suffix-icon': this.icon,
              readonly: this.readonly,
              size: this.size
            },
            attrs: {
              placeholder: this.placeholder,
              maxlength: this.maxLength,
              minlength: this.minLength,
              autofocus: this.autoFocus
            },
            on: {
              input: val => {
                this.$emit('input', val)
              }
            }
          },
          [
            this.$slots.front ? createElement('div', {
              slot: 'prepend'
            }, this.$slots.front) : null,
            this.$slots.end ? createElement('div', {
              slot: 'append'
            }, this.$slots.end) : null
          ]
        )
      ]
    )
  }
}
</script>