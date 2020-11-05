<!--
编辑模板（抽屉形式打开时的模板）

版本：1.0.0
作者：maqu
记录：
  20200219 新建
  20200302 新增编辑模板拖拉处理
           光标设置第一个控件

-->
<template>
  <el-drawer v-drag-border :title="title" :visible.sync="visible" direction="rtl" :wrapper-closable="closeOnClickModal"
             :show-close="true"
             :size="width" :modal="true" :close-on-press-escape="true" :destroy-on-close="true" :before-close="handleBeforeClose"
             @opened="handleOpened"
  >
    <iframe v-if="useiFrame" :src="url" :style="{height: height +'px',width:width}" />
    <drawer-inner-form v-else ref="editForm" :is-component-model="true" :url="url" @form-closed="handleBeforeClose" />
  </el-drawer>
</template>

<script>

  export default {
    name: 'DrawerWrapper',
    directives: {
      dragBorder: {
        bind(el, binding, vnode, oldVnode) {
          // 弹框可拉伸最小宽高
          const minWidth = 400

          // 弹窗
          const dragDom = el.querySelector('.el-drawer')

          // 给弹窗加上overflow auto；不然缩小时框内的标签可能超出dialog；
          dragDom.style.overflow = 'auto'

          dragDom.onmousemove = function(e) {
            if (e.clientX > dragDom.offsetLeft + dragDom.clientWidth - 10 || dragDom.offsetLeft + 10 > e.clientX) { // 向左拉
              dragDom.style.cursor = 'w-resize'
              // } else if (el.scrollTop + e.clientY > dragDom.offsetTop + dragDom.clientHeight - 10) {
              //   dragDom.style.cursor = 's-resize'
            } else {
              dragDom.style.cursor = 'default'

              dragDom.onmousedown = null
            }

            dragDom.onmousedown = (e) => {
              const clientX = e.clientX
              const clientY = e.clientY
              const elW = dragDom.clientWidth
              const EloffsetLeft = dragDom.offsetLeft
              const EloffsetTop = dragDom.offsetTop

              dragDom.style.userSelect = 'none'

              // 判断点击的位置是不是为头部
              if (clientX > EloffsetLeft && clientX < EloffsetLeft + elW && clientY > EloffsetTop && clientY <
                EloffsetTop + 100) {
                // 如果是头部在此就不做任何动作，以上有绑定dialogHeaderEl.onmousedown = moveDown;
              } else {
                document.onmousemove = function(e) {
                  e.preventDefault() // 移动时禁用默认事件
                  // 左侧鼠标拖拽位置
                  if (clientX > EloffsetLeft && clientX < EloffsetLeft + 10) {
                    // 往左拖拽
                    if (clientX > e.clientX) {
                      // console.log(elW)  弹出窗原始大小
                      // console.log(clientX)  原来距离左边距离
                      // console.log(e.clientX) 拖动时距离左边距离
                      dragDom.style.width = elW + (clientX - e.clientX) + 'px'
                    }

                    // 往右拖拽
                    if (clientX < e.clientX) {
                      if (dragDom.clientWidth >= minWidth) {
                        dragDom.style.width = elW - (e.clientX - clientX) + 'px'
                      }
                    }
                  }

                  // 右侧鼠标拖拽位置
                  if (clientX > EloffsetLeft + elW - 10 && clientX < EloffsetLeft + elW) {
                    // 往左拖拽
                    if (clientX > e.clientX) {
                      if (dragDom.clientWidth >= minWidth) {
                        dragDom.style.width = elW - (clientX - e.clientX) + 'px'
                      }
                    }

                    // 往右拖拽
                    if (clientX < e.clientX) {
                      dragDom.style.width = elW + (e.clientX - clientX) + 'px'
                    }
                  }
                }

                // 拉伸结束
                document.onmouseup = function(e) {
                  document.onmousemove = null

                  document.onmouseup = null
                }
              }
            }
          }
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
      return {}
    },
    methods: {
      handleCancel() {
        this.$emit('form-closed')
      },
      handleBeforeClose(e) {
        this.$emit('form-closed', e)
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
