<script>
export default {
  name: 'TkNumberRange',
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
      type: Number,
      default: ''
    },
    readonly: {
      type: Boolean
    },
    max: {
      type: Number
    },
    min: {
      type: Number
    },
    numbertype: {
      type: String
    },
    afterpoint: {
      type: Number
    },
    size: {
      type: String
    },
    step: {
      type: Number,
      default: 1
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
              'el-date-editor el-range-editor el-input__inner el-date-editor--numberrange': true,
              'is-active': this.isActive,
              [this.rangeSizeClass]: this.size
            }
          },
          [
            createElement(
              'el-input-number',
              {
                props: {
                  value: this.value,
                  placeholder: this.placeholder,
                  readonly: this.readonly,
                  max: this.max,
                  min: this.min,
                  precision: this.afterpoint,
                  step: this.step,
                  'controls-position': 'right'
                },
                on: {
                  focus: () => {
                    this.isActive = true
                  },
                  blur: () => {
                    this.isActive = false
                  },
                  input: val => {
                    this.$emit('input', val)
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
              'el-input-number',
              {
                props: {
                  value: this.endValue,
                  placeholder: this.placeholder,
                  readonly: this.readonly,
                  max: this.max,
                  min: this.min,
                  precision: this.afterpoint,
                  step: this.step,
                  'controls-position': 'right'
                },
                on: {
                  focus: () => {
                    this.isActive = true
                  },
                  blur: () => {
                    this.isActive = false
                  },
                  input: val => {
                    this.$emit('update:endValue', val)
                  }
                }
              }
            )
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
.el-date-editor--numberrange {
  // width: 380px;
  padding: 3px 0;
  ::v-deep
    .el-input-number__decrease:hover:not(.is-disabled)
    ~ .el-input
    .el-input__inner:not(.is-disabled),
  ::v-deep
    .el-input-number__increase:hover:not(.is-disabled)
    ~ .el-input
    .el-input__inner:not(.is-disabled) {
    border-color: #dcdfe6;
  }
  ::v-deep .el-input-number.is-controls-right {
    &:first-child .el-input__inner {
      border-radius: 4px 0 0 4px;
      border-left: none;
    }
    &:last-child .el-input__inner {
      border-radius: 0 4px 4px 0;
      border-right: none;
    }
  }
  &.is-active ::v-deep {
    .el-input-number.is-controls-right .el-input__inner,
    .el-input-number__decrease:hover:not(.is-disabled)
      ~ .el-input
      .el-input__inner:not(.is-disabled),
    .el-input-number__increase:hover:not(.is-disabled)
      ~ .el-input
      .el-input__inner:not(.is-disabled) {
      border-color: #409eff #dcdfe6;
    }
  }
}
</style>