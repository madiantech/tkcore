<!--
生成时的注意事项：
1 UR_SUB_PART等表名需要替换
2 空白行数据需要替换到newLineTemplates
3 el-from的ref需要保持以SubForm结尾
-->
<template>
  <div class="app-container mq-multi-detail-page">

    <!-- 内容区域 -->
    <div ref="detailForm"
         class="section-content"
         data-url="/c/xml/update/razortest/multiedit?id=%id%"
    >

      <el-collapse ref="collapse" v-model="activeNames" class="collapse-wrapper">

        <el-collapse-item collapse="true" name="UR_PART" class="collapse-item-wrapper">

          <template slot="title">
            <div class="collapse-item-title">UR_PART表名</div>
          </template>

          <el-form
            v-if="data['UR_PART']"
            ref="UR_PARTSubForm"
            :model="data" label-width="100px" class="one-row-wrapper"
          >
            <!-- 后台只返回一条 -->
            <div>
              <el-row class="row-wrapper">
                <el-col :span="16" class="col-wrapper">
                  <el-form-item label="Name" :prop="'UR_PART.0.Name'">
                    <div class="content text" :title="data['UR_PART'][0].Name">{{ data['UR_PART'][0].Name }}</div>
                  </el-form-item>
                </el-col>
              </el-row>
              <el-row class="row-wrapper">
                <el-col :span="16" class="col-wrapper">
                  <el-form-item label="Description" :prop="'UR_PART.0.Description'">
                    <div class="content text" :title="data['UR_PART'][0].Description">{{ data['UR_PART'][0].Description }}</div>
                  </el-form-item>
                </el-col>
              </el-row>
            </div>

          </el-form>

        </el-collapse-item>

        <el-collapse-item collapse="true" name="UR_SUB_PART" class="collapse-item-wrapper">

          <template slot="title">
            <div class="collapse-item-title">UR_SUB_PART表名</div>
          </template>

          <el-form
            v-if="data['UR_SUB_PART']"
            ref="UR_SUB_PARTSubForm"
            :model="data"
            label-width="0px"
            class="multi-row-wrapper"
          >
            <el-table
              ref="UR_SUB_PART"
              class="table"
              :data="data['UR_SUB_PART']" element-loading-text="Loading" border fit
            >
              <el-table-column type="index" width="55" align="center" show-overflow-tooltip />
              <el-table-column label="Name" width="120" show-overflow-tooltip>
                <template slot-scope="scope">
                  <el-form-item :prop="'UR_SUB_PART.'+ scope.$index + '.Name'">
                    <div :title="scope.row.Name">{{ scope.row.Name }}</div>
                  </el-form-item>
                </template>
              </el-table-column>
              <el-table-column label="Description" show-overflow-tooltip>
                <template slot-scope="scope">
                  <el-form-item :prop="'UR_SUB_PART.'+ scope.$index + '.Description'">
                    <div :title="scope.row.Name">{{ scope.row.Description }}</div>
                  </el-form-item>
                </template>
              </el-table-column>
            </el-table>

          </el-form>

        </el-collapse-item>

      </el-collapse>

    </div>

    <!-- 按钮区域 -->
    <div class="section-button">
      <el-button v-if="settings.modifyButtonVisible" v-tk-drawer-url="'multi-edit?id=1'" type="primary" icon="el-icon-edit">修改</el-button>
      <el-button v-if="settings.backButtonVisible" icon="el-icon-back" @click="handleBack">返回</el-button>
    </div>

    <!--modal形式打开的修改页面-->
    <dialog-wrapper :visible="dialogVisible" :url="dialogUrl" :width="dialogWidth" :title="dialogTitle" :height="dialogHeight"
                    @form-closed="handleDialogClosed"
    />

    <!--右侧抽屉形式打开的修改页面-->
    <drawer-wrapper :visible="drawerVisible" :url="drawerUrl" :width="drawerWidth" :title="drawerTitle" @form-closed="handleDrawerClosed" />

  </div>

</template>

<script src="./multi-detail-js.js"></script>

<style lang="scss" src="./multi-detail-css.scss"></style>
