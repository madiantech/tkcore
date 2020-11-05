<template>
  <el-dialog
    class="tk-dialog"
    :title="options.title"
    :visible.sync="visible"
    :width="options.width"
    :fullscreen="options.fullscreen"
    @open="open"
    @close="$emit('close')"
  >
    <!-- <slot /> -->
    <el-scrollbar style="height:100%">
      <div class="scrollbar-content">
        <div ref="slot"></div>
        <!-- <router-view /> -->
      </div>
    </el-scrollbar>

    <span
      slot="footer"
      class="dialog-footer"
    >
      <el-button @click="cancelFunc()">取 消</el-button>
      <el-button
        type="primary"
        @click="confirmFunc()"
      >确 定</el-button>
    </span>
  </el-dialog>
</template>
<script>
import popupMixin from '../../mixins/popup'
export default {
  name: 'tkDialog',
  mixins: [popupMixin]
}
</script>
<style scoped>
.tk-dialog >>> .el-dialog {
  margin: 20px auto !important;
  height: 90%;
  overflow: hidden;
  display: flex;
  flex-direction: column;
}
.tk-dialog >>> .el-dialog .el-dialog__body {
  overflow: auto;
  padding: 0;
  flex: 1;
}
.tk-dialog .scrollbar-content {
  padding: 30px 20px;
}
</style>