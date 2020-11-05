<template>
  <el-drawer
    class="tk-drawer"
    :title="options.title"
    :visible.sync="visible"
    :size="options.width"
    @open="open"
    @close="$emit('close')"
  >
    <el-scrollbar style="height:100%">
      <div class="drawer-body">
        <div ref="slot"></div>
        <!-- <router-view /> -->
      </div>
    </el-scrollbar>

    <div class="drawer-footer">
      <el-button @click="cancelFunc()">取 消</el-button>
      <el-button
        type="primary"
        @click="confirmFunc()"
      >确 定</el-button>
    </div>

  </el-drawer>
</template>
<script>
import popupMixin from '../../mixins/popup'
export default {
  name: 'tkDialog',
  mixins: [popupMixin]
  // a 
}
</script>
<style scoped>
.tk-drawer >>> .el-drawer__title {
  margin-bottom: 0;
}
.tk-drawer >>> .el-drawer__header > :first-child {
  outline: none;
}
.tk-drawer >>> .el-drawer__body {
  overflow: hidden;
  display: flex;
  flex-direction: column;
}
.tk-drawer .drawer-body {
  padding: 30px 20px;
}
.tk-drawer .drawer-footer {
  text-align: right;
  padding: 10px 20px 20px;
}
</style>