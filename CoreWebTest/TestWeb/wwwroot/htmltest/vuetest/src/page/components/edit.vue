    <template>
        <el-dialog title="编辑"
                   :visible.sync="dialogVisible"
                   width="30%"
                   @open="open()">
            <el-form :model="formData"
                     label-width="80px">
                <el-form-item label="名称">
                    <el-input v-model="formData.Name"></el-input>
                </el-form-item>
                <el-form-item label="描述">
                    <el-input v-model="formData.Description"></el-input>
                </el-form-item>
            </el-form>
            <span slot="footer"
                  class="dialog-footer">
                <el-button @click="dialogVisible = false">取 消</el-button>
                <el-button type="primary"
                           @click="confirm()">确 定</el-button>
            </span>
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
            data() {
                return {
                    formData: {}
                }
            },
            computed: {
                dialogVisible: {
                    get() {
                        return this.visible
                    },
                    set(val) {
                        this.$emit('update:visible', val)
                    }
                }
            },
            methods: {
                open() {
                    this.formData = Object.assign({}, this.data)
                },
                confirm() {
                    this.$emit('confirm', this.formData)
                    this.dialogVisible = false
                }
            }
        }
    </script>
