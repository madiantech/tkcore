<!--
详情模板

功能：
  含修改和返回按钮的详情查看模板

特性：
1 标题长度超度超出结尾用省略号
2 文本内容长度超度超出结尾用省略号
3 通过控制el-row和el-col来实现布局
4 修改页面支持三种效果：v-edit-in-modal 弹框,v-edit-in-drawer 右侧推出,v-edit-in-route 路由跳转

版本：1.0.0
作者：maqu
记录：
  20200220 新建

-->
<template>
  <div class="app-container mq-detail-page">
    <!-- 内容区域 -->
    <div ref="detailForm"
         class="section-content"
         data-url="/c/xml/detail/razortest/partdata?id=%id%"
         data-table-name="UR_PART"
    >

      <!--      &lt;!&ndash; 行的封套 &ndash;&gt;-->
      <!--      <el-row class="row-wrapper">-->
      <!--        &lt;!&ndash; 列的封套 &ndash;&gt;-->
      <!--        <el-col :span="8" class="col-wrapper">-->
      <!--          &lt;!&ndash; 标题 &ndash;&gt;-->
      <!--          <div class="title" title="field1">field1:</div>-->
      <!--          &lt;!&ndash; 内容 &ndash;&gt;-->
      <!--          <div class="content text" title="123456789012345678901234567890">1234567890123456789012345678901234567890-->
      <!--          </div>-->
      <!--        </el-col>-->
      <!--        <el-col :span="8" class="col-wrapper">-->
      <!--          <div class="title">field2:</div>-->
      <!--          <div class="content text">1234567890</div>-->
      <!--        </el-col>-->
      <!--        <el-col :span="8" class="col-wrapper">-->
      <!--          <div class="title">field3:</div>-->
      <!--          <div class="content text">1234567890</div>-->
      <!--        </el-col>-->
      <!--      </el-row>-->

      <!-- 行的封套 -->
      <el-row class="row-wrapper">
        <!-- 列的封套 -->
        <el-col :span="16" class="col-wrapper">
          <!-- 标题 -->
          <div class="title" title="Name">Name:</div>
          <!-- 内容 -->
          <div class="content text" :title="data.Name">{{ data.Name }}</div>
        </el-col>
        <!-- 列的封套 -->
        <el-col :span="16" class="col-wrapper">
          <!-- 标题 -->
          <div class="title" title="Description">Description:</div>
          <!-- 内容 -->
          <div class="content text" :title="data.Description">{{ data.Description }}</div>
        </el-col>
      </el-row>
    </div>

    <!-- 按钮区域 -->
    <div class="section-button">
      <el-button v-if="settings.modifyButtonVisible" v-tk-dialog-url="'edit?id=1'" type="primary" icon="el-icon-edit">修改</el-button>
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

<script src="./detail-js.js"></script>

<style scoped lang="scss" src="./detail-css.scss"></style>
