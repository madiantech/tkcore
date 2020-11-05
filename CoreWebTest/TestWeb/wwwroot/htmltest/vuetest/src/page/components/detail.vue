    <template>
        <el-dialog title="详情" :visible.sync="dialogVisible" width="30%">
            <el-form :model="data" label-width="80px">
                <el-form-item label="名称">
                    {{data.Name}}
                </el-form-item>
                <el-form-item label="描述">
                    {{data.Description}}
                </el-form-item>
            </el-form>
        </el-dialog>
    </template>
<script>
export default {
  props: {
    visible: {
      type: Boolean,
      default: false
    },
    data: {
      type: Object,
      default: () => { }
    }
  },
  computed: {
    dialogVisible: {
      get () {
        return this.visible
      },
      set (val) {
        this.$emit('update:visible', val)
      }
    }
  },
}
</script>
