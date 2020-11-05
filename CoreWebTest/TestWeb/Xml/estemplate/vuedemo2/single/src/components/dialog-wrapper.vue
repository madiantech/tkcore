<!--
编辑模板（弹窗形式打开时的模板）

版本：1.0.0
作者：maqu
记录：
  20200219 新建
  20200302 新增编辑模板拖拉处理
           光标设置第一个控件

-->
<template>
  <el-dialog v-fullscreen :title="title" :visible.sync="visible" :close-on-click-modal="closeOnClickModal" :show-close="true"
             :width="width" :modal="true" :lock-scroll="true" :close-on-press-escape="true" :destroy-on-close="false"
             :before-close="handleBeforeClose" :fullscreen="fullscreen" style="overflow: auto;"
             append-to-body @opened="handleOpened"
  >
    <iframe v-if="useiFrame" :src="url" :style="{height: height +'px',width:width}" />
    <dialog-inner-form v-else ref="editForm" :is-component-model="true" :url="url" @form-closed="handleBeforeClose" />
  </el-dialog>
</template>

<script>
  export default {
    name: 'DialogWrapper',
    directives: {
      fullscreen: {
        bind(el, binding, vnode, oldVnode) {
          // 弹窗
          const dragDom = el.querySelector('.el-dialog')

          // 获取弹框头部（这部分可双击全屏）
          const dialogHeaderEl = el.querySelector('.el-dialog__header')
          dialogHeaderEl.onselectstart = new Function('return false')

          dialogHeaderEl.ondblclick = function(e) {
            vnode.context.fullscreen = !vnode.context.fullscreen
          }
          // 给弹窗加上overflow auto；不然缩小时框内的标签可能超出dialog；
          dragDom.style.overflow = 'auto'
        }
      }
    },
    props: {
      // 是否显示
      visible: {
        type: Boolean,
        required: true,
        default: false
      },
      // url
      url: {
        type: String,
        default: ''
      },
      // 标题
      title: {
        type: String,
        default: ''
      },
      // 宽度
      width: {
        type: String,
        default: '30%'
      },
      // 高
      height: {
        type: Number,
        default: 300
      },
      // 是否使用iFrame
      useiFrame: {
        type: Boolean,
        default: false
      },
      // 是否可以通过点击 modal 关闭 Dialog
      closeOnClickModal: {
        type: Boolean,
        default: false
      }
    },
    data() {
      return {
        fullscreen: false
      }
    },
    methods: {
      handleCancel() {
        this.$emit('form-closed')
        this.fullscreen = false
      },
      handleBeforeClose(e) {
        // console.log('handleBeforeClose')
        this.$emit('form-closed', e)
        this.fullscreen = false
      },
      handleOpened() {
        if (this.$refs.editForm.$el) {
          const element = this.$refs.editForm.$el.querySelector('.el-input')
          if (element) {
            element.firstElementChild.focus()
          }
        }
      }

    }
  }
</script>

<style scoped lang="scss">
</style>
