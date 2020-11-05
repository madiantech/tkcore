<template>
  <div>
    <el-form inline>
      <tk-input
        name="test1"
        size="medium"
        caption="测试"
        tooltip="测试tooltip"
        required
      />
      <tk-input
        name="test2"
        size="medium"
      />
      <el-form-item size="medium">
        <el-button
          type="default"
          @click="handleSizeChange()"
        >搜索</el-button>
        <el-button
          type="primary"
          v-tk-dialog="{url:'/add'}"
        >添加</el-button>
        <el-button type="success">导出</el-button>
      </el-form-item>
    </el-form>
    <el-tabs
      v-model="activeName"
      @tab-click="search"
    >
      <el-tab-pane
        label="用户管理"
        name="user"
      />
      <el-tab-pane
        label="测试"
        name="test"
      />
    </el-tabs>
    <el-table
      :data="tableData"
      @sort-change="sortChange"
      ref="UR_PART"
      url="http://localhost:5000/c/xml/list/Test/TestPartVueList?Type=Data"
    >
      <el-table-column
        prop="Id"
        label="ID"
        sortable="custom"
      >
      </el-table-column>
      <el-table-column
        prop="Name"
        label="名称"
      >
      </el-table-column>
      <el-table-column
        prop="Description"
        label="描述"
      >
      </el-table-column>
      <el-table-column label="操作">
        <template slot-scope="{row}">
          <!-- <el-button
            type="text"
            size="small"
            @click="alertURL(row)"
          >alertURL</el-button> -->
          <el-button
            type="text"
            size="small"
            v-tk-dialog="{id:row.Id,url:'url',data:row,component:detail}"
          >查看</el-button>
          <el-button
            v-if="buttonShow('Update',row._OPERATOR_RIGHT)"
            type="text"
            size="small"
            v-tk-drawer="{id:row.Id,url:'url',data:row,component:edit,options:{confirm:editConfirm}}"
          >编辑</el-button>
          <!-- <el-button
            v-if="showDel(row)"
            type="text"
            size="small"
          >删除</el-button> -->
        </template>
      </el-table-column>
    </el-table>
    <el-pagination
      class="list-pagination"
      @size-change="handleSizeChange"
      @current-change="handleCurrentChange"
      :current-page="Page.CurrentPage+1"
      :page-sizes="[10, 15, 20, 25,30]"
      :page-size.sync="Page.PageSize"
      layout="total, sizes, prev, pager, next, jumper"
      :total="Page.TotalCount"
    >
    </el-pagination>
  </div>
</template>
<script>
import edit from '../components/edit'
import add from '../components/add'
import detail from '../components/tabDetail'
export default {
  components: {
    add
  },
  data () {
    return {
      tableData: [],
      currentObj: {},
      RowOperator: [],
      Page: {
        TotalCount: 0,
        TotalPage: 0,
        CurrentPage: 0,
        PageSize: 15
      },
      bodyData: {},
      detail: detail,
      edit: edit,
      activeName: 'user'
    }
  },
  mounted () {
    const tableName = Object.keys(this.$refs)[0]
    this.bodyData = {
      url: this.$refs[tableName].$attrs.url,
      tableName: tableName
    }
    this.search()
  },
  methods: {
    handleSizeChange () {
      this.search()
    },
    handleCurrentChange (val) {
      this.Page.CurrentPage = val - 1
      this.search()
    },
    sortChange ({ column, prop, order }) {
      console.log(column, prop, order)
      let Sort = 0
      if (order === 'ascending') Sort = 1
      else if (order === 'descending') Sort = -1
      this.search({ Order: prop, Sort })
    },
    search (option = {}) {
      this.$axios.get(this.bodyData.url, {
        params: Object.assign({}, option, {
          Page: this.Page.CurrentPage,
          PageSize: this.Page.PageSize
        })
      }).then(res => {
        this.tableData = res.data[this.bodyData.tableName]
        // 列表按钮
        this.RowOperator = res.data.RowOperator
        // 分页数据
        const countObj = res.data.Count[0] || {}
        Object.keys(countObj).forEach(key => {
          this.Page[key] = Number(countObj[key])
        })
      })
    },
    buttonShow (type, permision) {
      return permision.indexOf(type) > -1
    },
    alertURL (row) {
      alert(`/url?Id=${row.Id}`)
      // 
    },
    editConfirm (data) {
      const obj = data.data
      let item = this.tableData.find(ele => ele.Id === obj.Id)
      item && Object.assign(item, obj)
    },
    addConfirm (obj) {
      this.tableData.push(Object.assign({}, obj, { Id: this.tableData[this.tableData.length - 1].Id + 1 }))
    }
  }
}
</script>