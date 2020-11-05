<!--
列表模板

功能：
  含增删改查功能的列表模板

特性：
1 重置按钮可设置隐藏
2 查询重置按钮可设置显示位置，支持两种：底部(bottom) or 跟随（inline）
3 查询条件过多时自动换行
4 全局按钮过多时自动换行
5 新增和修改页面支持三种效果：v-tk-dialog-url 弹框,v-tk-drawer-url 右侧推出,v-tk-url 路由跳转；

版本：1.0.0
作者：maqu
记录：
  20200219 新建

-->
<template>

  <div class="app-container mq-list-page">

    <!--TAB切换区域-->
    <el-tabs v-model="activeTabName" @tab-click="handleTabClick">
      <el-tab-pane label="用户管理" name="first" />
      <el-tab-pane label="配置管理" name="second" />
    </el-tabs>

    <!--查询条件区域-->
    <div class="section-condition">

      <div class="fields-wrapper">
        <el-form ref="first" :inline="true" :model="condition" label-width="100px">

          <!-- 搜索条件字段 -->
          <el-form-item label="姓名" prop="Name" class="field-item">
            <el-input v-model="condition.Name" placeholder="姓名" />
          </el-form-item>
          <el-form-item label="描述" prop="Description" class="field-item">
            <el-input v-model="condition.Description" placeholder="描述" />
          </el-form-item>

          <!-- 搜索条件按钮 -->
          <el-form-item v-if="settings.conditionButtonPosition ==='inline'" class="field-item">
            <el-checkbox v-model="isEqual">精确查询</el-checkbox>
            <el-button type="primary" icon="el-icon-search" @click="handleSearch">查询</el-button>
            <el-button v-if="settings.conditionResetButtonVisible" icon="el-icon-delete" @click="handleReset('first')">重置
            </el-button>
          </el-form-item>
        </el-form>

        <!-- 搜索条件按钮 -->
        <div v-if="settings.conditionButtonPosition !=='inline'" class="buttons-wrapper">
          <el-checkbox v-model="isEqual">精确查询</el-checkbox>
          <el-button type="primary" icon="el-icon-search" @click="handleSearch">查询</el-button>
          <el-button v-if="settings.conditionResetButtonVisible" icon="el-icon-delete" @click="handleReset('second')">
            重置
          </el-button>
        </div>
      </div>

    </div>

    <!--全局操作按钮区域-->
    <div shadow="always" class="section-global-button">
      <!--TODO:全局操作栏按钮平铺，下拉，混合-->
      <el-button v-tk-dialog-url="{url:'add',title:'新增',width:'40%',height:300,}" class="button-item" type="success" icon="el-icon-plus">添加</el-button>
    </div>

    <!--数据显示区域-->
    <div class="section-datatable">
      <el-table
        ref="table"
        v-loading="dataLoading"
        class="table" :header-cell-class-name="handleTheadAddClass"
        data-url="/c/xml/list/RazorTest/Part"
        data-table-name="UR_PART"
        :data="dataset" element-loading-text="Loading" border fit highlight-current-row
        stripe @sort-change="handleSortChange"
      >
        <el-table-column align="center" label="Id" width="95" prop="Id" />

        <el-table-column label="Name" prop="Name" width="110" sortable="custom">
          <template slot-scope="scope">
            <div v-tk-drawer-url="{url:'detail?id=*Id*',title:'查看详情',width:'40%',height:300,parseSource:scope.row,closeOnClick:true}" v-html="scope.row.Name" />
          </template>
        </el-table-column>
        <el-table-column label="Description" prop="Description" sortable="custom">
          <template slot-scope="scope">
            <div v-html="scope.row.Description" />
          </template>
        </el-table-column>
        <el-table-column class-name="action-col" label="操作" align="center">
          <!--TODO:操作栏按钮平铺，下拉，混合-->
          <template slot-scope="scope">
            <el-button v-if="hasOperatorRight(scope.row,'Delete')" v-tk-ajax-url="{url:'delete?id=*Id*',confirm:'此操作将永久删除, 是否继续?',method:'post',parseSource:scope.row}" class="button-item" size="mini" type="danger">删除</el-button>
            <el-button v-if="hasOperatorRight(scope.row,'Update')" v-tk-dialog-url="{url:'edit?id=*Id*',title:'修改',width:'40%',height:300,parseSource:scope.row}" class="button-item" size="mini" type="primary">修改</el-button>
            <el-button v-if="hasOperatorRight(scope.row,'Update')" v-tk-drawer-url="{url:'edit?id=*Id*',title:'修改',width:'40%',height:300,parseSource:scope.row}" class="button-item" size="mini" type="primary">修改</el-button>
            <el-button v-if="hasOperatorRight(scope.row,'Update')" v-tk-drawer-url="{url:'multi-edit?id=*Id*',title:'修改',width:'40%',height:300,parseSource:scope.row}" class="button-item" size="mini" type="primary">修改</el-button>
            <el-button v-if="hasOperatorRight(scope.row,'Detail')" v-tk-url="{url:'https://www.baidu.com',confirm:'确定吗',newwindow:true,withRetUrl:true,parseSource:scope.row}" class="button-item" size="mini" type="info">查看</el-button>
          </template>
        </el-table-column>
      </el-table>
    </div>

    <!--翻页-->
    <div class="section-pagination">
      <el-pagination background :hide-on-single-page="false" :current-page="pagination.currentPage" :page-sizes="pagination.pageSizes"
                     :page-size="pagination.pageSize" layout="total, sizes, prev, pager, next, jumper" :total="pagination.total"
                     @size-change="handleSizeChange" @current-change="handleCurrentChange"
      />
    </div>

    <!--modal形式打开的修改页面-->
    <dialog-wrapper :visible="dialogVisible" :url="dialogUrl" :width="dialogWidth" :title="dialogTitle" :height="dialogHeight"
                    :close-on-click-modal="dialogCloseOnClickModal"
                    @form-closed="handleDialogClosed"
    />

    <!--右侧抽屉形式打开的修改页面-->
    <drawer-wrapper :visible="drawerVisible" :url="drawerUrl" :width="drawerWidth" :title="drawerTitle"
                    :close-on-click-modal="drawerCloseOnClickModal"
                    @form-closed="handleDrawerClosed"
    />

  </div>

</template>

<script src="./list-js.js"></script>

<style lang="scss" src="./list-css.scss"></style>
