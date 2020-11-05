<template>
  <el-form-item
    :prop="name"
    :class="classname"
    :label="caption"
    :label-width="labelWidth"
    :required="required"
    :size="size"
  >
    <div class="multiple-easysearch">
      <el-select
        v-model="input"
        :disabled="readonly"
        :placeholder="placeholder"
        clearable
        filterable
        remote
        multiple
        automatic-dropdown
        value-key="Value"
        :remote-method="remoteMethod"
        :loading="loading"
        @change="change"
      >
        <el-option
          v-for="item in options"
          :key="item.Value"
          :label="item.Name"
          :value="item"
        >
        </el-option>
      </el-select>
      <div class="easysearch-button">
        <el-button
          icon="el-icon-search"
          @click="visible=true"
        ></el-button>
      </div>
    </div>

    <el-tooltip
      v-if="tooltip"
      :content="tooltip"
    >
      <el-button
        type="text"
        class="icon-help"
        icon="el-icon-question"
      />
    </el-tooltip>
    <el-dialog
      :title="`选择${caption}...`"
      :visible.sync="visible"
      @open="dialogOpen"
    >
      <el-input
        v-model="dialogInput"
        @keyup.enter.native="dialogSearch"
      >
        <el-button
          slot="append"
          icon="el-icon-search"
          @click="dialogSearch"
        />
      </el-input>
      <el-scrollbar
        tag="ul"
        class="el-select-dropdown is-multiple"
        wrap-class="el-select-dropdown__wrap"
        view-class="el-select-dropdown__list"
        style="height:200px"
      >
        <li
          class="el-select-dropdown__item"
          :class="{selected:input.some(obj=>obj.Value===item.Value)}"
          v-for="item in dialogOptions"
          :key="item.Value"
          @click="dialogSelect(item)"
        ><span>{{item.Name}}</span></li>
      </el-scrollbar>
    </el-dialog>
  </el-form-item>
</template>
<script>
import { FormItem, Select, Tooltip } from 'element-ui'
export default {
  name: 'TkMultipleEasysearch',
  components: {
    [FormItem.name]: FormItem,
    [Select.name]: Select,
    [Tooltip.name]: Tooltip
  },
  props: {
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
      // this.input = item.Value
      // this.text = item.Name
      const i = this.input.findIndex(obj => obj.Value === item.Value)
      i > -1 ? this.hidden.splice(i, 1) : this.hidden.push(item.Value)
      this.visible = false
    },
    change (val) {
      if (this.afterSelect) this.afterSelect()
    }
  }
}
</script>
<style lang="scss" scoped>
.multiple-easysearch {
  line-height: normal;
  display: inline-table;
  width: 100%;
  border-collapse: separate;
  border-spacing: 0;
  .el-select {
    vertical-align: middle;
    display: table-cell;
    ::v-deep.el-input__inner {
      border-top-right-radius: 0;
      border-bottom-right-radius: 0;
    }
  }
  .easysearch-button {
    background-color: #f5f7fa;
    color: #909399;
    vertical-align: middle;
    display: table-cell;
    position: relative;
    border: 1px solid #dcdfe6;
    border-left: 0;
    border-radius: 0 4px 4px 0;
    padding: 0 20px;
    width: 1px;
    white-space: nowrap;
    .el-button {
      border-color: transparent;
      background-color: transparent;
      color: inherit;
      border-top: 0;
      border-bottom: 0;
      // font-size: inherit;
      // display: inline-block;
      margin: -10px -20px;
    }
  }
}
</style>