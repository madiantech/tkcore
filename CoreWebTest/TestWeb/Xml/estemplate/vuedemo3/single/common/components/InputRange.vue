<script>
export default {
  name: 'TkInputRange',
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
    },
    rangeSeparator: {
      type: String,
      default: '-'
    },
    endValue: {
      type: [String, Number],
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
          'div',
          {
            class: {
              'el-date-editor el-range-editor el-input__inner el-date-editor--inputrange': true,
              'is-active': this.isActive,
              [this.rangeSizeClass]: this.size
            }
          },
          [
            createElement(
              'input',
              {
                class: "el-range-input",
                attrs: {
                  autocomplete: "off",
                  placeholder: this.placeholder,
                  maxlength: this.maxLength,
                  minlength: this.minLength,
                  disabled: this.disabled,
                  readonly: this.readonly,
                  value: this.value
                },
                on: {
                  focus: () => {
                    this.isActive = true
                  },
                  blur: () => {
                    this.isActive = false
                  },
                  input: e => {
                    this.$emit('input', e.target.value)
                  }
                }
              }
            ),
            createElement(
              'span',
              {
                class: 'el-range-separator'
              },
              this.rangeSeparator
            ),
            createElement(
              'input',
              {
                class: "el-range-input",
                attrs: {
                  autocomplete: "off",
                  placeholder: this.placeholder,
                  maxlength: this.maxLength,
                  minlength: this.minLength,
                  disabled: this.disabled,
                  readonly: this.readonly,
                  value: this.endValue
                },
                on: {
                  focus: () => {
                    this.isActive = true
                  },
                  blur: () => {
                    this.isActive = false
                  },
                  input: e => {
                    this.$emit('update:endValue', e.target.value)
                  }
                }
              }
            ),
            this.icon ? createElement('i', { class: icon }) : null
          ]
        )
      ]
    )
  },
  data () {
    return {
      isActive: false
    }
  },
  computed: {
    rangeSizeClass () {
      return `el-range-editor--${this.size}`
    }
  }
}
</script>
<style lang="scss" scoped>
.el-date-editor--inputrange {
  // width: 350px;
  justify-content: space-around;
  .el-range-input {
    text-align: left;
  }
}
</style>