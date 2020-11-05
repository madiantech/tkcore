<template>
  <div>
    <el-tabs type="border-card">
      <el-tab-pane label="详情">
        <el-form
          :model="formData"
          label-width="80px"
        >
          <el-form-item label="ID">
            {{formData.Id}}
          </el-form-item>
          <el-form-item label="名称">
            {{formData.Name}}
          </el-form-item>
          <el-form-item label="描述">
            {{formData.Description}}
          </el-form-item>
        </el-form>
      </el-tab-pane>
      <el-tab-pane label="测试">测试</el-tab-pane>
    </el-tabs>
    <el-divider></el-divider>
    <el-card header="详情">
      <el-form
        :model="formData"
        label-width="80px"
      >
        <el-form-item label="ID">
          {{formData.Id}}
        </el-form-item>
        <el-form-item label="名称">
          {{formData.Name}}
        </el-form-item>
        <el-form-item label="描述">
          {{formData.Description}}
        </el-form-item>
      </el-form>
    </el-card>
  </div>

</template>
<script>
export default {
  name: 'listDetail',
  // data () {
  // return {
  //   data: null // data为指令的值
  // }
  // }, 
  computed: {
    formData () {
      return this.data.data
    }
  },
  mounted () {
    //    
    console.log(this)
  }
}
</script>