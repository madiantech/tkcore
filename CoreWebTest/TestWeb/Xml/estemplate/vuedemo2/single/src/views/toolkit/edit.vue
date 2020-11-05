<!--
编辑模板（新增和编辑通用）

功能：
  含确定、重置和返回按钮的详情查看模板

特性：
1 重置按钮、返回按钮可设置隐藏
2 通过控制el-row和el-col来实现布局

版本：1.0.0
作者：maqu
记录：
  20200221 新建

-->
<template>
  <div class="app-container mq-edit-page">
    <!-- 内容区域 -->
    <div class="section-content">

      <el-form
        ref="editForm"
        :model="data"
        :rules="rules"
        label-width="100px"
        data-url="/c/xml/update/RazorTest/Part?id=%id%"
        data-table-name="UR_PART"
      >

         <!-- 无Group的场合 -->
        <div>
          <!-- 行的封套 -->
          <el-row class="row-wrapper">
            <!-- 列的封套 -->
            <el-col :span="16" class="col-wrapper">
              <el-form-item label="Name" prop="Name" :error="errors.Name">
                <el-input v-model="data.Name" />
              </el-form-item>
            </el-col>
          </el-row>

          <!-- 行的封套 -->
          <el-row class="row-wrapper">
            <!-- 列的封套 -->
            <el-col :span="16" class="col-wrapper">
              <el-form-item label="Description" prop="Description" :error="errors.Description">
                <el-input v-model="data.Description" />
              </el-form-item>
            </el-col>
          </el-row>
        </div>

        <!-- 有Group的场合 -->
        <el-collapse ref="collapse" v-model="activeNames" class="collapse-wrapper">

          <el-collapse-item name="Group1" collapse="true" class="collapse-item-wrapper" >

            <template slot="title">
              <div class="collapse-item-title">Group1</div>
            </template>

            <!-- 行的封套 -->
            <el-row class="row-wrapper">
              <!-- 列的封套 -->
              <el-col :span="16" class="col-wrapper">
                <el-form-item label="Name" prop="Name" :error="errors.Name">
                  <el-input v-model="data.Name" />
                </el-form-item>
              </el-col>
            </el-row>

            <!-- 行的封套 -->
            <el-row class="row-wrapper">
              <!-- 列的封套 -->
              <el-col :span="16" class="col-wrapper">
                <el-form-item label="Description" prop="Description" :error="errors.Description">
                  <el-input v-model="data.Description" />
                </el-form-item>
              </el-col>
            </el-row>

          </el-collapse-item>

          <el-collapse-item name="Group2" collapse="true" class="collapse-item-wrapper">

            <template slot="title">
              <div class="collapse-item-title">Group2</div>
            </template>

            <!-- 行的封套 -->
            <el-row class="row-wrapper">
              <!-- 列的封套 -->
              <el-col :span="16" class="col-wrapper">
                <el-form-item label="Name" prop="Name" :error="errors.Name">
                  <el-input v-model="data.Name" />
                </el-form-item>
              </el-col>
            </el-row>

            <!-- 行的封套 -->
            <el-row class="row-wrapper">
              <!-- 列的封套 -->
              <el-col :span="16" class="col-wrapper">
                <el-form-item label="Description" prop="Description" :error="errors.Description">
                  <el-input v-model="data.Description" />
                </el-form-item>
              </el-col>
            </el-row>

          </el-collapse-item>

        </el-collapse>

      </el-form>

    </div>

    <!-- 按钮区域 -->
    <div class="section-button">
      <el-button type="primary" @click="handleSubmitForm('editForm')">确定</el-button>
      <el-button v-if="settings.resetButtonVisible" icon="el-icon-delete" @click="handleReset('editForm')">重置</el-button>
      <el-button v-if="settings.backButtonVisible" @click="handleBack">{{ settings.backButtonCaption }}</el-button>
    </div>

  </div>

</template>

<script src="./edit-js.js"></script>

<style scoped lang="scss" src="./edit-css.scss"></style>
