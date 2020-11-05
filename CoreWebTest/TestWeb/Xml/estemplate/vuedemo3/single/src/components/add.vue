<template>
  <div
    ref="current"
    url="http://localhost:5000/c/xml/insert/Test/TestPartVueList?Type=Data"
  >
    <el-card header="用户">
      <el-form
        :model="UR_PART"
        label-width="80px"
      >
        <tk-hidden
          name="Id"
          v-model="UR_PART.Id"
        />
        <el-row>
          <el-col :span="24">
            <!-- <el-form-item label="名称">
              <el-input
                v-model="UR_PART.Name"
                placeholder="名称"
              ></el-input>
              
            </el-form-item> -->
            <tk-input
              name="Name"
              caption="名称"
              required
              v-model="UR_PART.Name"
            ></tk-input>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="24">
            <el-form-item label="描述">
              <el-input
                v-model="UR_PART.Description"
                placeholder="描述"
              ></el-input>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <div style="text-align:center">
        <el-button
          type="primary"
          size="medium"
          plain
          @click="confirm"
        >保存</el-button>
        <el-button
          type="info"
          size="medium"
          plain
          @click="cancel"
        >取消</el-button>
      </div>
    </el-card>
    <el-divider></el-divider>
    <el-card :body-style="{ padding: '0' }">
      <template #header>
        用户从表
        <el-button-group style="float:right">
          <el-button
            type="primary"
            size="mini"
            round
            @click="handleAdd('UR_SUB_PART')"
          >新建</el-button>
          <el-button
            type="primary"
            size="mini"
            round
            @click="handleSelectAll('UR_SUB_PART')"
          >全选</el-button>
          <el-button
            type="primary"
            size="mini"
            round
            @click="handleSelectReverse('UR_SUB_PART')"
          >反选</el-button>
          <el-button
            type="primary"
            size="mini"
            round
            @click="handleSelectClear('UR_SUB_PART')"
          >全不选</el-button>
          <el-button
            type="primary"
            size="mini"
            round
            @click="handleDelete('UR_SUB_PART')"
          >删除</el-button>
          <el-button
            type="primary"
            size="mini"
            round
            @click="handleClear('UR_SUB_PART')"
          >全删</el-button>
        </el-button-group>
      </template>
      <el-table
        ref="UR_SUB_PART"
        :data="UR_SUB_PART"
        @selection-change="val=>{handleSelectionChange('UR_SUB_PART',val)}"
      >
        <el-table-column
          type="selection"
          align="center"
          width="55"
        >
        </el-table-column>
        <el-table-column
          label="名称"
          prop="Name"
        >
          <el-input
            slot-scope="{row}"
            size="medium"
            v-model="row.Name"
            placeholder="名称"
          >
          </el-input>
        </el-table-column>
        <el-table-column
          prop="Description"
          label="描述"
        >
          <el-input
            slot-scope="{row}"
            size="medium"
            v-model="row.Description"
            placeholder="描述"
          >
          </el-input>
        </el-table-column>
      </el-table>
    </el-card>
  </div>
</template>
<script>
import editMixin from 'common/mixins/edit'
export default {
  mixins: [editMixin]
}
</script>