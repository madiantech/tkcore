<template>
  <el-form-item
    :prop="name"
    :class="classname"
    :label="caption"
    :label-width="labelWidth"
    :required="required"
    :size="size"
  >
    <el-autocomplete
      v-model="input"
      :disabled="readonly"
      :placeholder="placeholder"
      value-key="Name"
      clearable
      :fetch-suggestions="querySearchAsync"
      @change="change"
    >
      <el-button
        slot="append"
        icon="el-icon-search"
        @click="visible=true"
      ></el-button>
    </el-autocomplete>

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
        class="el-select-dropdown"
        wrap-class="el-select-dropdown__wrap"
        view-class="el-select-dropdown__list"
        style="height:200px"
      >
        <li
          class="el-select-dropdown__item"
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
  name: 'TkEasysearchText',
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
  computed: {
    input: {
      get () {
        return this.value
      },
      set (val) {
        this.$emit('input', val)
      }
    }
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
      this.input = item.Value
      this.visible = false
    },
    change (val) {
      if (this.afterSelect) this.afterSelect()
    }
  }
}
</script>