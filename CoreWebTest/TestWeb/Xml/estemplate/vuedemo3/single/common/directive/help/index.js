import Vue from 'vue'
export default {
  bind: (el, binding, vnode) => {
    if (!binding.value) return
    let vm = new (Vue.extend({
      template: `
      <el-tooltip
        v-if="tooltip"
        :content="tooltip"
      >
        <el-button
          type="text"
          class="icon-help"
          icon="el-icon-question"
        />
      </el-tooltip>
      `,
      data () {
        return {
          tooltip: binding.value
        }
      }
    }))
    vm.$mount()
    el.appendChild(vm.$el)
    el.style.position = 'relative'
  }
}
