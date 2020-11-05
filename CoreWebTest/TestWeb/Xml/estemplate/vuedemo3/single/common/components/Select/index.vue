<script>
export default {
  name: 'TkSelect',
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
    readonly: {
      type: Boolean
    },
    value: {
      type: [String, Number],
      default: ''
    },
    placeholder: {
      type: String,
      default: '请输入...'
    },
    clearable: {
      type: Boolean,
      default: true
    },
    addition: {
      type: Object
    },
    size: {
      type: String
    },
    noDataText: {
      type: String
    },
    tooltip: {
      type: String
    },
    required: {
      type: Boolean
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
          'el-select',
          {
            props: {
              value: this.value,
              disabled: this.readonly,
              placeholder: this.placeholder,
              clearable: this.clearable,
              'no-data-text': this.noDataText,
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
              'tk-combo-item',
              {
                props: {
                  value: item.Value,
                  text: item.Name,
                  readonly: item.readonly,
                  addition: item.addition
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