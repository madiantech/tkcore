<template>
  <div class="tree-container">
    <div class="tree-main">
      <el-input
        placeholder="输入关键字进行过滤"
        v-model="filterText"
      >
      </el-input>
      <el-tree
        ref="tree"
        :props="props"
        :load="loadNode"
        :default-expanded-keys="[]"
        :default-checked-keys="[]"
        :filter-node-method="filterNode"
        lazy
        draggable
        show-checkbox
      />
    </div>
    <el-form
      class="tree-detail"
      label-width="80px"
    >
      <el-row>
        <el-col :span="8">
          <el-form-item label="ID">
            ID
          </el-form-item>
        </el-col>
        <el-col :span="8">
          <el-form-item label="名称">
            Name
          </el-form-item>
        </el-col>
        <el-col :span="8">
          <el-form-item label="描述">
            Desc
          </el-form-item>
        </el-col>
      </el-row>
    </el-form>
  </div>

</template>

<script>
export default {
  data () {
    return {
      props: {
        label: 'name',
        children: 'zones',
        isLeaf: 'leaf'
      },
      filterText: ''
    };
  },
  watch: {
    filterText (val) {
      this.$refs.tree.filter(val);
    }
  },
  methods: {
    loadNode (node, resolve) {
      //  
      if (node.level === 0) {
        return resolve([{ name: 'region' }]);
      }
      if (node.level > 1) return resolve([]);

      setTimeout(() => {
        const data = [{
          name: 'leaf',
          leaf: true
        }, {
          name: 'zone'
        }];
        // 
        resolve(data);
      }, 500);
    },
    filterNode (value, data) {
      if (!value) return true;
      return data.name.indexOf(value) !== -1;
    }
  }
};
</script>
<style lang="scss" scoped>
.tree-container {
  display: flex;

  .tree-main {
    min-width: 300px;
  }
  .tree-detail {
    flex: 1;
  }
}
</style>