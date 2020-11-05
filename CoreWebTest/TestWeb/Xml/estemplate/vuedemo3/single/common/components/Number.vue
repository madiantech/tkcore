<script>
export default {
  name: 'TkNumber',
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