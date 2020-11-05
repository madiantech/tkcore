<script>
export default {
  name: 'TkMultipleEasysearch',
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
    readonly: {
      type: Boolean
    },
    labelWidth: {
      type: String
    },
    value: {
      type: Object,
      default: ''
    },
    hiddenValue: {
      type: String,
      default: ''
    },
    placeholder: {
      type: String,
      default: '请输入...'
    },
    classname: {
      type: String
    },
    url: {
      type: String,
      default: ''
    },
    dialogUrl: {
      type: String,
      default: ''
    },
    refField: { //[{"Field":"PartField",RefField:"Part"}]
      type: Array
    },
    regName: {
      type: String
    },
    addition: {
      type: Object
    },
    afterSelect: {
      type: Function
    },
    size: {
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
          'div',
          {
            class: 'multiple-easysearch easysearch'
          },
          [
            createElement(
              'el-select',
              {
                props: {
                  value: this.input,
                  disabled: this.readonly,
                  placeholder: this.placeholder,
                  clearable: true,
                  filterable: true,
                  remote: true,
                  multiple: true,
                  'automatic-dropdown': true,
                  'value-key': 'Value',
                  'remote-method': this.remoteMethod,
                  loading: this.loading
                },
                on: {
                  input: val => {
                    this.input = val
                  },
                  change: this.change
                }
              },
              this.options.map(item => {
                return createElement(
                  'el-option',
                  {
                    props: {
                      label: item.Name,
                      value: item
                    }
                  }
                )
              })
            ),
            createElement(
              'div',
              {
                class: 'easysearch-button'
              },
              [
                createElement(
                  'el-button',
                  {
                    props: {
                      icon: `el-icon-search`
                    },
                    on: {
                      click: () => {
                        this.visible = true
                      }
                    }
                  }
                )
              ]
            )
          ]
        ),
        createElement(
          'el-dialog',
          {
            props: {
              title: `选择${this.caption}...`,
              visible: this.visible
            },
            on: {
              'update:visible': val => {
                this.visible = val
              },
              open: this.dialogOpen
            }
          },
          [
            createElement(
              'el-input',
              {
                props: {
                  value: this.dialogInput
                },
                on: {
                  input: val => {
                    this.dialogInput = val
                  }
                },
                nativeOn: {
                  keyup: e => {
                    if (e.keyCode === 13 || e.keyCode === 108) {
                      this.dialogSearch()
                    }
                  }
                }
              },
              [
                createElement(
                  'el-button',
                  {
                    slot: 'append',
                    props: {
                      icon: "el-icon-search"
                    },
                    on: {
                      click: this.dialogSearch
                    }
                  }
                )
              ]
            ),
            createElement(
              'el-scrollbar',
              {
                class: 'el-select-dropdown is-multiple',
                style: {
                  height: '200px'
                },
                props: {
                  tag: "ul",
                  'wrap-class': "el-select-dropdown__wrap",
                  'view-class': "el-select-dropdown__list"
                }
              },
              this.dialogOptions.map(item => {
                return createElement(
                  'li',
                  {
                    class: {
                      'el-select-dropdown__item': true,
                      selected: this.input.some(obj => obj.Value === item.Value)
                    },
                    on: {
                      click: () => {
                        this.dialogSelect(item)
                      }
                    }
                  },
                  item.Name
                )
              })
            )
          ]
        )
      ]
    )
  },
  computed: {
    input: {
      get () {
        return this.value && this.value.Data ? this.value.Data : []
      },
      set (val) {
        this.$emit('input', { 'Data': val })
      }
    },
    hidden: {
      get () {
        return this.hiddenValue.split(',')
      },
      set (val) {
        this.$emit('update:hiddenValue', val.join(','))
      }
    }
  },
  data () {
    return {
      options: [],
      list: [],
      loading: false,
      states: ["Alabama", "Alaska", "Arizona",
        "Arkansas", "California", "Colorado",
        "Connecticut", "Delaware", "Florida",
        "Georgia", "Hawaii", "Idaho", "Illinois",
        "Indiana", "Iowa", "Kansas", "Kentucky",
        "Louisiana", "Maine", "Maryland",
        "Massachusetts", "Michigan", "Minnesota",
        "Mississippi", "Missouri", "Montana",
        "Nebraska", "Nevada", "New Hampshire",
        "New Jersey", "New Mexico", "New York",
        "North Carolina", "North Dakota", "Ohio",
        "Oklahoma", "Oregon", "Pennsylvania",
        "Rhode Island", "South Carolina",
        "South Dakota", "Tennessee", "Texas",
        "Utah", "Vermont", "Virginia",
        "Washington", "West Virginia", "Wisconsin",
        "Wyoming"],
      visible: false,
      dialogInput: '',
      dialogOptions: []
    }
  },
  mounted () {
    this.list = this.states.map((item, index) => {
      return { Value: index, Name: item }
    })
    this.remoteMethod('')
  },
  methods: {
    remoteMethod (query) {
      this.loading = true
      setTimeout(() => {
        this.loading = false
        this.options = this.list.filter(item => {
          return item.Name.toLowerCase()
            .indexOf(query.toLowerCase()) > -1
        })
      }, 200)
    },
    dialogOpen () {
      this.dialogInput = ''
      this.dialogSearch()
    },
    dialogSearch () {
      this.dialogOptions = this.list.filter(item => {
        return item.Name.toLowerCase()
          .indexOf(this.dialogInput.toLowerCase()) > -1
      })
    },
    dialogSelect (item) {
      const arr = [...this.input]
      const i = arr.findIndex(obj => obj.Value === item.Value)
      if (i > -1) {
        this.hidden.splice(i, 1)
        arr.splice(i, 1)
      } else {
        this.hidden.push(item.Value)
        arr.push(item)
        if (!this.options.some(obj => obj.Value === item.Value)) this.options.push(item)
      }
      this.$set(this, 'input', arr)
      this.visible = false
    },
    change (val) {
      if (this.afterSelect) this.afterSelect()
    }
  }
}
</script>
<style lang="scss" scoped>
@import './easysearch.scss';
</style>