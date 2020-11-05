<!--
新增模板（新增和编辑通用）

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

      <el-form ref="editForm" :model="data" :rules="rules" label-width="100px">

        <!-- 行的封套 -->
        <el-row class="row-wrapper">
          <!-- 列的封套 -->
          <el-col :span="8" class="col-wrapper">
            <el-form-item label="Field1" prop="f1">
              <el-input v-model="data.f1" />
            </el-form-item>
          </el-col>
          <el-col :span="8" class="col-wrapper">
            <el-form-item label="Field2" prop="f2">
              <el-input v-model="data.f2" />
            </el-form-item>
          </el-col>
          <el-col :span="8" class="col-wrapper">
            <el-form-item label="Field3" prop="f3">
              <el-input v-model="data.f3" />
            </el-form-item>
          </el-col>
        </el-row>

        <!-- 行的封套 -->
        <el-row class="row-wrapper">
          <!-- 列的封套 -->
          <el-col :span="16" class="col-wrapper">
            <el-form-item label="Field4" prop="f4">
              <el-input v-model="data.f4" />
            </el-form-item>
          </el-col>
          <el-col :span="8" class="col-wrapper">
            <el-form-item label="Field5" prop="f5">
              <el-input v-model="data.f5" />
            </el-form-item>
          </el-col>
        </el-row>

        <!-- 行的封套 -->
        <el-row class="row-wrapper">
          <!-- 列的封套 -->
          <el-col :span="16" class="col-wrapper">
            <el-form-item label="Field4" prop="f4">
              <el-input v-model="data.f4" />
            </el-form-item>
          </el-col>
          <el-col :span="8" class="col-wrapper">
            <el-form-item label="Field5" prop="f5">
              <el-input v-model="data.f5" />
            </el-form-item>
          </el-col>
        </el-row>

        <!-- 行的封套 -->
        <el-row class="row-wrapper">
          <!-- 列的封套 -->
          <el-col :span="16" class="col-wrapper">
            <el-form-item label="Field4" prop="f4">
              <el-input v-model="data.f4" />
            </el-form-item>
          </el-col>
          <el-col :span="8" class="col-wrapper">
            <el-form-item label="Field5" prop="f5">
              <el-input v-model="data.f5" />
            </el-form-item>
          </el-col>
        </el-row>

        <!-- 行的封套 -->
        <el-row class="row-wrapper">
          <!-- 列的封套 -->
          <el-col :span="16" class="col-wrapper">
            <el-form-item label="Field4" prop="f4">
              <el-input v-model="data.f4" />
            </el-form-item>
          </el-col>
          <el-col :span="8" class="col-wrapper">
            <el-form-item label="Field5" prop="f5">
              <el-input v-model="data.f5" />
            </el-form-item>
          </el-col>
        </el-row>

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

<script>
  export default {
    props: {
      // 是否显示
      isComponentModel: {
        type: Boolean,
        default: false
      }
    },
    data() {
      return {
        // 页面设置
        settings: {
          resetButtonVisible: false,
          backButtonVisible: true,
          backButtonCaption: '返回'
        },
        dataLoading: true, // 数据加载中
        // 数据
        data: {
          f1: '',
          f2: '',
          f3: '',
          f4: '',
          f5: '',
          f6: ''
        },
        // 验证规则（toolkit是否不需要这块）
        rules: {
          f1: [
            {
              required: true,
              message: 'please input f1',
              trigger: 'blur'
            }],
          f2: [
            {
              required: true,
              message: 'please input f1',
              trigger: 'blur'
            }]
        }

      }
    },
    mounted() {
      this.settings.backButtonCaption = this.isComponentModel ? '关闭' : '返回'
    },
    methods: {
      handleSubmitForm(formName) {
        this.$refs[formName].validate((valid) => {
          if (valid) {
            alert('submit!')
          } else {
            return false
          }
        })

        if (this.isComponentModel) {
          this.$emit('form-closed')
        }
      },
      handleReset(formName) {
        this.$refs[formName].resetFields()
      },
      handleBack() {
        if (this.isComponentModel) {
          this.$emit('form-closed')
        } else {
          this.$router.go(-1)
        }
      }
    }
  }
</script>

<style scoped lang="scss">

  // 编辑页面
  .mq-edit-page {

    // 内容区域
    .section-content {
      overflow: auto;
      min-width: 400px;
      min-height: 300px;

      // 行
      .row-wrapper {
        // 列
        .col-wrapper {

        }
      }
    }

    // 按钮区域
    .section-button {
      clear: both;
      margin-top: 20px;
      text-align: center;
    }
  }

</style>
