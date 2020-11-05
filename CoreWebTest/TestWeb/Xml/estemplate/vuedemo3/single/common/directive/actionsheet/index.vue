<template>
  <div
    class="sheet-mask"
    @click="closetest"
  >

    <transition
      name="fade"
      mode="out-in"
    >
      <section
        v-show="visible"
        class="sheet-container"
        id="sheetContainer"
        :style="'width:' + options.width||'50%'"
      >
        <section class="sheet-head">
          <el-row :gutter="10">
            <el-col :span="12">
              <section class="operationBox flex">
                <section class="w100 flex">
                  <el-button
                    @click="close()"
                    size="mini"
                    type="text"
                    icon="el-icon-close"
                  />
                </section>
                <section class="flex_1">
                  <slot name="btns"></slot>
                </section>
              </section>
            </el-col>
            <el-col :span="12">
              <section class="mainMsgBox"></section>
            </el-col>
          </el-row>
        </section>
        <section class="sheet-body scroller-style scroller-style-min">
          <section id="sheetBody">
            <slot name="base">
              <!-- <new-vue
                            ref="newVue"
                            :template="currentAsyncTemp"
                            el="sheetBody"
                            :parentVm="thisVm"
                            :data="{ pageData: currentForm, resData: currentResData, queryForm: {}, pageType: 'form' }"
                            :methods="hooks.methods"
                        ></new-vue>-->
              <div ref="slot"></div>
              <!-- <router-view /> -->
            </slot>
          </section>
        </section>
      </section>
    </transition>
  </div>
</template>
<script>
import popupMixin from '../../mixins/popup'
export default {
  name: 'tkActionsheet',
  mixins: [popupMixin],
  mounted () {
    // this.$nextTick(() => {
    //     this.visible = true
    // })
    // setTimeout(() => { this.visible = true }, 1000)
    this.open()
  },
  methods: {
    closetest (e) {
      var btn = document.querySelector(".sheet-container");
      if (btn) {
        if (!btn.contains(e.target)) {            //按钮.app-download以外的区域
          this.close()
        }
      }
    },
    close () {
      this.visible = false
      setTimeout(() => { this.$emit('close') }, 300)
    }
  }
}
</script>
<style lang="scss" scoped>
.sheet-mask {
  position: fixed;
  top: 0;
  bottom: 0;
  right: 0;
  left: 0;
  z-index: 1;
}
.sheet-container {
  position: absolute;
  height: 85vh;
  width: 50vw;
  right: 0px;
  top: 50%;
  transform: translate(0, -50%);
  border: 1px solid #e0e0e0;
  background: #fff;
  z-index: 100;
  box-shadow: 0px 3px 4px #ccc;
  overflow: hidden;
  .sheet-head {
    width: 100%;
    min-height: 40px;
    padding: 5px 10px;
    border-bottom: 1px solid #e0e0e0;
    z-index: 2;
    .mainMsgBox {
      min-height: 40px;
      width: 100%;
    }
    .operationBox {
      height: 40px;
      line-height: 40px;
      width: 100%;
    }
  }
  .sheet-body {
    z-index: 1;
    padding: 5px 20px;
    height: 100%;
    overflow: auto;
    padding-bottom: 60px;
  }
}
// 动画
.fade-leave-active,
.fade-enter-active {
  transition: all 0.3s;
}
.fade-enter,
.fade-leave-to {
  opacity: 0;
  transform: translate(50%, -50%);
}
// end 动画
</style>
