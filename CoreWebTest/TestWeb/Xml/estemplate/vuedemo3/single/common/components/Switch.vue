<script>
export default {
  name: 'TkSwitch',
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
    width: {
      type: Number
    },
    onText: {
      type: String,
      default: '开'
    },
    offText: {
      type: String,
      default: '关'
    },
    onValue: {
      type: String,
      default: '1'
    },
    offValue: {
      type: String,
      default: '0'
    },
    onColor: {
      type: String,
      default: '#20A0FF'
    },
    offColor: {
      type: String,
      default: '#C0CCDA'
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
          'el-switch',
          {
            class: 'switch-text_in',
            props: {
              value: this.value,
              disabled: this.readonly,
              width: this.trueWidth,
              'active-text': this.onText,
              'inactive-text': this.offText,
              'active-value': this.onValue,
              'inactive-value': this.offValue,
              'active-color': this.onColor,
              'inactive-color': this.offColor
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
  },
  computed: {
    trueWidth () {
      return this.width || (this.onText || this.offText ? 58 : 46)
    }
  }
}
</script>
<style lang="scss" scoped>
.switch-text_in {
  ::v-deep.el-switch__label--left {
    position: absolute;
    z-index: -1;
    color: #fff;
    left: 50%;
    top: 50%;
    transform: translate(0%, -50%);
    &.is-active {
      z-index: 1;
    }
  }
  ::v-deep.el-switch__label--right {
    position: absolute;
    color: #fff;
    z-index: -1;
    right: 50%;
    top: 50%;
    transform: translate(0, -50%);
    &.is-active {
      z-index: 1;
    }
  }
}
</style>