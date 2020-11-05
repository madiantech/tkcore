<template>
  <div
    ref="current"
    url="http://localhost:5000/c/xml/detail/Test/TestPartVueList?Type=Data"
  >
    <el-card header="用户">
      <el-form
        :model="UR_PART"
        label-width="80px"
      >
        <el-form-item label="ID">
          <div v-html="UR_PART.Id"></div>
        </el-form-item>
        <el-form-item label="名称">
          <div v-html="UR_PART.Name"></div>
        </el-form-item>
        <el-form-item label="描述">
          <div v-html="UR_PART.Description"></div>
        </el-form-item>
      </el-form>
      <div style="text-align:center">
        <el-button
          type="primary"
          size="medium"
          icon="el-icon-edit"
          plain
          @click="edit()"
        >修改节点</el-button>
        <el-button
          type="danger"
          size="medium"
          icon="el-icon-delete"
          plain
          @click="del()"
        >删除节点</el-button>
        <el-button
          type="primary"
          size="medium"
          icon="el-icon-plus"
          plain
          @click="addChildNode()"
        >新建子节点</el-button>
      </div>
    </el-card>
    <el-divider></el-divider>
    <el-tabs
      ref='tabs'
      type="border-card"
      v-model="activeName"
      @tab-click="handleClick"
    >
      <el-tab-pane
        label="用户从表"
        name="UR_SUB_PART"
      >
        <el-table :data="UR_SUB_PART">
          <el-table-column
            label="名称"
            prop="Name"
          >
            <div
              slot-scope="{row}"
              v-html="row.Name"
            >
            </div>
          </el-table-column>
          <el-table-column
            prop="Description"
            label="描述"
          >
            <div
              slot-scope="{row}"
              v-html="row.Description"
            >
            </div>
          </el-table-column>
        </el-table>
      </el-tab-pane>
    </el-tabs>
  </div>
</template>
<script>
import detailMixin from 'common/mixins/detail'
export default {
  mixins: [detailMixin]
}
</script>