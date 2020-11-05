<template>
    <div>
        <el-button size="small"
                   type="primary"
                   @click="add()">添加</el-button>
        <el-table :data="tableData">
                <el-table-column prop="Name"
                                 label="名称">
                </el-table-column>
                <el-table-column prop="Description"
                                 label="描述">
                </el-table-column>
            <el-table-column label="操作">
                <template slot-scope="{row}">
                    <el-button type="text"
                               size="small"
                               @click="show(row)">查看</el-button>
                    <el-button type="text"
                               size="small"
                               @click="edit(row)">编辑</el-button>
                </template>
            </el-table-column>
        </el-table>
        <!-- 查看详情 -->
        <detail-dialog :visible.sync="detailDialogVisible"
                       :data="currentObj" />
        <!-- end 查看详情 -->
        <!-- 编辑 -->
        <edit-dialog :visible.sync="editDialogVisible"
                     :data="currentObj"
                     @confirm="editConfirm" />
        <!-- end 编辑 -->
        <add-dialog :visible.sync="addDialogVisible"
                    @confirm="addConfirm" />
    </div>
</template>
<script>
    import axios from 'axios'
    import detailDialog from './components/detail'
    import editDialog from './components/edit'
    import addDialog from './components/add'
    export default {
        components: {
            detailDialog,
            editDialog,
            addDialog
        },
        data() {
            return {
                tableData: [],
                currentObj: {},
                detailDialogVisible: false,
                editDialogVisible: false,
                addDialogVisible: false,
                bodyData: {}
            }
        },
        created() {
            this.bodyData = JSON.parse(document.body.dataset.json)
            this.search()

        },
        methods: {
            search() {
                axios.get(this.bodyData.dataUrl).then(res => {
                    this.tableData = res.data[this.bodyData.tableName]
                })
            },
            showEdit(row) {
                return row.Id % 2 === 0
            },
            showDel(row) {
                return row.Id % 3 === 0
            },
            show(row) {
                this.currentObj = Object.assign({}, row)
                this.detailDialogVisible = true
            },
            edit(row = { Id: null, Name: '', Desc: '' }) {
                this.currentObj = Object.assign({}, row)
                this.editDialogVisible = true
            },
            add() {
                this.addDialogVisible = true
            },
            alertURL(row) {
                alert(`/url?Id=${row.Id}`)
            },
            editConfirm(obj) {
                let item = this.tableData.find(ele => ele.Id === obj.Id)
                item && Object.assign(item, obj)
            },
            addConfirm(obj) {
                this.tableData.push(Object.assign({}, obj, { Id: this.tableData[this.tableData.length - 1].Id + 1 }))
            }
        }
    }
</script>