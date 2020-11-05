<!--
生成时的注意事项：
1 UR_SUB_PART等表名需要替换
2 空白行数据需要替换到newLineTemplates
3 el-from的ref需要保持以SubForm结尾
-->
<template>
  <div class="app-container mq-multi-edit-page">

    <!-- 内容区域 -->
    <div ref="editForm"
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
            :model="data"
            :rules="rules['UR_PART']" label-width="100px" class="one-row-wrapper"
          >
            <!-- 后台只返回一条 -->
            <div>
              <el-row class="row-wrapper">
                <el-col :span="16" class="col-wrapper">
                  <el-form-item label="Name" :prop="'UR_PART.0.Name'" :error="errorText('UR_PART.0.Name')">
                    <el-input v-model="data['UR_PART'][0].Name" :value="data['UR_PART'][0].Name" />
                  </el-form-item>
                </el-col>
              </el-row>
              <el-row class="row-wrapper">
                <el-col :span="16" class="col-wrapper">
                  <el-form-item label="Description" :prop="'UR_PART.0.Description'" :error="errorText('UR_PART.0.Description')">
                    <el-input v-model="data['UR_PART'][0].Description" />
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
            :rules="rules['UR_SUB_PART']"
            label-width="0px"
            class="multi-row-wrapper"
          >
            <el-table
              ref="UR_SUB_PART"
              class="table"
              :data="data['UR_SUB_PART']" element-loading-text="Loading" border fit
            >
              <el-table-column type="selection" align="center" width="55" />
              <el-table-column type="index" width="55" align="center" show-overflow-tooltip />
              <el-table-column label="Name" width="120" show-overflow-tooltip>
                <template slot-scope="scope">
                  <el-form-item :prop="'UR_SUB_PART.'+ scope.$index + '.Name'" :error="errorText('UR_SUB_PART.'+ scope.$index + '.Name')" >
                    <el-input v-model="scope.row.Name" />
                  </el-form-item>
                </template>
              </el-table-column>
              <el-table-column label="Description" show-overflow-tooltip>
                <template slot-scope="scope">
                  <el-form-item :prop="'UR_SUB_PART.'+ scope.$index + '.Description'" :error="errorText('UR_SUB_PART.'+ scope.$index + '.Description')" >
                    <el-input v-model="scope.row.Description" />
                  </el-form-item>
                </template>
              </el-table-column>
            </el-table>

            <!-- 按钮区域 -->
            <el-row class="buttons-wrapper" justify="space-between">
              <el-col :span="16" class="btn-group">
                <el-button class="button-item ui-btn-check-all" type="primary" icon="" size="mini" @click="handleCheckAll('UR_SUB_PART')">全选</el-button>
                <el-button class="button-item ui-btn-check-reverse" type="primary" icon="" size="mini" @click="handleCheckReverse('UR_SUB_PART')">反选</el-button>
                <el-button class="button-item ui-btn-uncheck-all" type="primary" icon="" size="mini" @click="handleUncheckAll('UR_SUB_PART')">全不选</el-button>
                <el-button class="button-item ui-btn-delete-row" type="warning" icon="" size="mini" @click="handleDeleteRow('UR_SUB_PART')">删除</el-button>
                <el-button class="button-item ui-btn-delete-all" type="danger" icon="" size="mini" @click="handleDeleteAll('UR_SUB_PART')">全删</el-button>
              </el-col>
              <el-col :span="8" class="ui-new-row">
                <el-dropdown size="mini" split-button type="primary" class="button-item" @command="handleCommand('UR_SUB_PART',$event)" @click="handleCommand('UR_SUB_PART','1')">
                  新建
                  <el-dropdown-menu slot="dropdown">
                    <el-dropdown-item command="2">x 2</el-dropdown-item>
                    <el-dropdown-item command="3">x 3</el-dropdown-item>
                    <el-dropdown-item command="4">x 4</el-dropdown-item>
                    <el-dropdown-item command="5">x 5</el-dropdown-item>
                    <el-dropdown-item command="custom">自定义</el-dropdown-item>
                  </el-dropdown-menu>
                </el-dropdown>
              </el-col>
            </el-row>
          </el-form>

        </el-collapse-item>

      </el-collapse>

    </div>

    <!-- 按钮区域 -->
    <div class="section-button">
      <el-button type="primary" @click="handleSubmitForm('editForm')">确定</el-button>
      <el-button v-if="settings.resetButtonVisible" icon="el-icon-delete" @click="handleReset('editForm')">重置</el-button>
      <el-button v-if="settings.backButtonVisible" @click="handleBack">{{ settings.backButtonCaption }}</el-button>
    </div>

    <!-- 新增数据行的模板 -->
    <script ref="newLineTemplates" type="text/template">
      {
      "UR_PART": {
      "Id": "",
      "Name": "",
      "Description": ""
      },
      "UR_SUB_PART": {
      "Id": "",
      "PartId": "",
      "Name": "",
      "Description": ""
      }
      }
    </script>

  </div>

</template>

<script src="./multi-edit-js.js"></script>

<style lang="scss" src="./multi-edit-css.scss"></style>
