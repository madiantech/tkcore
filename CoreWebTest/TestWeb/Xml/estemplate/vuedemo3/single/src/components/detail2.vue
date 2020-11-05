<template>
  <div
    ref="current"
    url="http://localhost:5000/c/xml/detail/Test/TestPartVueList?Type=Data&Id=*Id*"
  >
    <el-tabs
      ref='tabs'
      type="border-card"
      v-model="activeName"
      @tab-click="handleClick"
    >
      <el-tab-pane
        label="用户主表"
        name="UR_PART"
        loaded="true"
      >
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
      </el-tab-pane>
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
  mixins: [detailMixin],
}
</script>