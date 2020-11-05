<template>
  <div
    ref="current"
    url="/testData/error.json"
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
    </el-card>
    <el-divider></el-divider>
    <el-card
      header="用户从表"
      name="subTable"
      tablename="UR_SUB_PART"
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
    </el-card>
    <el-divider></el-divider>
    <el-card
      header="test"
      name="subTable"
      tablename="test"
    >
    </el-card>
  </div>
</template>
<script>
import detailMixin from 'common/mixins/detail'
export default {
  mixins: [detailMixin],
}
</script>