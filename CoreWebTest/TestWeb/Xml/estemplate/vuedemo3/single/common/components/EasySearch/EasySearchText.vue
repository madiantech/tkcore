<script>
export default {
  name: 'TkEasysearchText',
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
      type: [String, Number],
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
          'el-autocomplete',
          {
            props: {
              value: this.value,
              disabled: this.readonly,
              placeholder: this.placeholder,
              'value-key': "Name",
              clearable: true,
              'fetch-suggestions': this.querySearchAsync
            },
            on: {
              input: val => {
                this.$emit('input', val)
              },
              change: this.change
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
                  click: () => {
                    this.visible = true
                  }
                }
              }
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
                class: 'el-select-dropdown',
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
                    class: 'el-select-dropdown__item',
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
  data () {
    return {
      timeout: null,
      list: [],
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
      return { Value: item, Name: item }
    })
  },
  methods: {
    querySearchAsync (query, cb) {
      clearTimeout(this.timeout);
      this.timeout = setTimeout(() => {
        const results = this.list.filter(item => {
          return item.Name.toLowerCase()
            .indexOf(query.toLowerCase()) > -1
        })
        cb(results);
      }, 3000 * Math.random());
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
      this.$emit('input', item.Value)
      this.visible = false
    },
    change (val) {
      if (this.afterSelect) this.afterSelect()
    }
  }
}
</script>