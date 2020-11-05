import { getQueryStringArgs, replaceUriSegment } from 'common/utils/utils'
export default {
  data () {
    return {
      InitValue: '',
      idName: 'Id',
      parentIdName: 'ParentId',
      rootId: '-1',
      newNodeName: '新节点',
      tempUrl: '',
      treeData: [],
      treeKey: 0,
      props: {
        label: 'text',
        children: '_zones',
        isLeaf: function (data, node) {
          return !data.children
        }
      },
      filterText: '',
      temporary: 0,
      currentTreeData: {},
      defaultExpandedKeys: []
    };
  },
  watch: {
    filterText (val) {
      this.$refs.tree.filter(val)
    }
  },
  mounted () {
    this.tempUrl = this.$refs.current.getAttribute('url')
    this.InitValue = getQueryStringArgs(window.location.href).InitValue || ''
    this.refresh()
    this.$on('reset-current-node', () => {
      const key = this.$route.query[this.idName]
      this.$refs.tree.setCurrentKey(Number(key) < 0 ? Number(key) : key)
    })
  },
  methods: {
    refresh () {
      this.treeKey++
    },
    loadNode (node, resolve) {
      if (!this.tempUrl) return resolve([])
      if (node.data[this.props.children] && node.data[this.props.children].length > 0) return resolve(node.data[this.props.children])
      this.$axios.get(this.tempUrl, {
        params: {
          id: node.level === 0 ? '' : node.data.id,
          InitValue: node.level === 0 ? this.InitValue : ''
        }
      }).then(res => {
        if (node.level === 0 && this.InitValue) {
          this.getDefaultExpandNode(res, this.InitValue)
          const treeData = this.setTreeData(res, this.InitValue)
          const initData = res.find(item => item.id === this.InitValue)
          this.treeCurrentChange(initData)
          return resolve(treeData)
        } else {
          if (node.level === 0 && res.length > 0) {
            this.InitValue = res[0].id
            this.treeCurrentChange(res[0])
            this.$nextTick(() => {
              this.$refs.tree.setCurrentKey(res[0].id)
            })
          }
          return resolve(res)
        }
      }).catch(() => {
        return resolve([])
      })
    },
    getDefaultExpandNode (data, currentId) {
      const obj = data.find(item => item.id === currentId)
      if (obj && obj.parent !== '#') {
        this.defaultExpandedKeys.push(obj.parent)
        this.getDefaultExpandNode(data, obj.parent)
      }
    },
    setTreeData (data, currentId) {
      // 找到currentId全部同级节点
      const parentId = data.find(item => item.id === currentId).parent
      const arr = data.filter(item => item.parent === parentId)
      // 过滤出其他节点
      let other = data.filter(item => item.parent !== parentId)
      // 把同级节点挂到相应父节点下边
      const obj = other.find(item => item.id === parentId)
      obj[this.props.children] = [...arr]
      if (other.some(item => item.parent !== '#')) {
        other = this.setTreeData(other, parentId)
      }
      return other
    },
    filterNode (value, data) {
      if (!value) return true
      return data[this.props.label].indexOf(value) !== -1
    },
    dropoSameLevel (draggingNode, dropNode, type) {
      if (draggingNode.level === dropNode.level && draggingNode.parent.id === dropNode.parent.id) {
        // 向上拖拽 || 向下拖拽
        return type === "prev" || type === "next"
      } else {
        // 不同级进行处理
        return false
      }
    },
    treeCurrentChange (data, node) {
      data && (this.currentTreeData = data)
      if (this.currentTreeData['id'] === this.$route.query[this.idName]) return
      if (data['id'] < 0) {
        this.$router.push({
          path: '/add',
          query: {
            [this.idName]: data['id'],
            [this.parentIdName]: node.parent.level === 0 ? this.rootId : node.parent.data['id']
          }
        })
      } else {
        this.$router.push(`/detail?${this.idName}=${data['id']}`)
      }
    },
    treeNodeMove (direction) {
      const node = this.$refs.tree.getNode(this.currentTreeData)
      const childNodes = node.parent.childNodes
      const index = childNodes.findIndex(item => item.id === node.id)
      const url = replaceUriSegment(this.tempUrl, 'CMoveUpDown')
      this.$axios.get(url, { params: { direction, "Id": this.currentTreeData['id'] } }).then(res => {
        switch (direction) {
          case 'Up':
            if (index > 0) {
              const beforeNode = childNodes[index - 1]
              this.$refs.tree.remove(node.data)
              this.$refs.tree.insertBefore(node.data, beforeNode)
              this.$refs.tree.setCurrentNode(node.data)
            }
            break
          case 'Down':
            if (index < childNodes.length - 1) {
              const afterNode = childNodes[index + 1]
              this.$refs.tree.remove(node.data)
              this.$refs.tree.insertAfter(node.data, afterNode)
              this.$refs.tree.setCurrentNode(node.data)
            }
            break
        }
      }).catch(() => { })
    },
    // getTreeParent (data, value, nodeKey, childrenKey) {
    //   let result
    //   if (data && data.length > 0) {
    //     data.forEach(item => {
    //       if (item[childrenKey] && item[childrenKey].length > 0) {
    //         if (item[childrenKey].some(obj => obj[nodeKey] === value)) {
    //           result = item
    //         } else {
    //           result = this.getTreeParent(item[childrenKey], value, nodeKey, childrenKey)
    //         }
    //       }
    //     })
    //   }
    //   return result
    // },
    addRootNode () {
      if (this.temporary < 0 && this.$route.name === 'add') {
        this.$message({
          message: `当前有未保存的${this.newNodeName}`,
          type: 'warning'
        })
        return
      }
      this.$refs.tree.append({ id: --this.temporary, [this.props.label]: this.newNodeName }, null)
      this.$router.push({
        path: `/add`,
        query: {
          [this.idName]: this.temporary,
          [this.parentIdName]: this.rootId
        }
      })
    },
    nodeConfirm (res) {
      window.location.replace(res.Result.Message)
    },
    nodeCancel (bol) {
      const key = Number(this.$route.query[this.idName])
      key < 0 && this.$refs.tree.remove(key) && (this.temporary = 0)
      !bol && this.$router.back(-1)
    },
    nodeDeleted () {
      const key = this.$route.query[this.idName]
      this.$refs.tree.remove(key)
      this.$router.push('/')
    },
    appendChildNode (query) {
      // 展开当前节点 设为非叶子节点
      const node = this.$refs.tree.getNode(this.currentTreeData)
      node.expanded = true
      // node.isLeaf = false
      node.isLeafByUser = false
      // this.currentTreeData.children = true
      // if (this.currentTreeData.type === 'Leaf') this.currentTreeData.type = 'Branch'
      this.$refs.tree.append({ id: --this.temporary, [this.props.label]: this.newNodeName }, this.currentTreeData['id'])
      this.$router.push({
        path: `/add`,
        query: {
          [this.idName]: this.temporary,
          [this.parentIdName]: query[this.idName]
        }
      })
    }
  }
}