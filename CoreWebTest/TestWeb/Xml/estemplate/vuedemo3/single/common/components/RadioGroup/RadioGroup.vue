<script>
export default {
  name: 'TkRadioGroup',
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
    tooltip: {
      type: String
    },
    required: {
      type: Boolean
    },
    size: {
      type: String
    },
    url: {
      type: String
    },
    regName: {
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
          'el-radio-group',
          {
            props: {
              value: this.value,
              disabled: this.readonly,
              size: this.size
            },
            on: {
              input: val => {
                this.$emit('input', val)
              }
            }
          },
          // this.$slots.default
          this.url ? this.options.map(item => {
            return createElement(
              'tk-radio-item',
              {
                props: {
                  value: item.Value,
                  text: item.Name
                }
              }
            )
          }) : this.$slots.default
        )
      ]
    )
  },
  data () {
    return {
      options: []
    }
  },
  created () {
    if (this.url) {
      this.$axios.get(this.url).then(res => {
        this.options = res[this.regName]
      })
    }
  }
}
</script>