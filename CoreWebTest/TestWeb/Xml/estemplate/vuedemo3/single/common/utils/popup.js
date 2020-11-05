import Vue from 'vue'
export default function (component) {
  return function (el, binding, vnode) {
    el.onclick = function () {
      const value = binding.value
      if (!value) return
      const parentVm = vnode.context
      const routes = parentVm.$router.options.routes
      const path = value.url.split('?')[0]
      const route = getTreeRoute(routes, path)
      if (!route) return
      let vm = new (Vue.extend(component))
      vm.component = route.component
      if (value.options) { // 配置项单独保存
        Object.keys(value.options).forEach(key => {
          vm.options[key] = value.options[key]
        })
      }

      let data = {} // 除去配置项后 全部数据保存 统一给子组件
      Object.keys(value).forEach(key => {
        if (key !== 'options' && key !== 'component') {
          data[key] = value[key]
        }
      })
      vm.data = JSON.parse(JSON.stringify(data))// 深拷贝
      vm.$mount()
      document.body.appendChild(vm.$el)
      vm.$on('close', () => {
        vm.$el.parentNode.removeChild(vm.$el);
        vm.$destroy()
      })
      vm.$on('confirm', () => {
        parentVm.$emit('popupConfirm')
      })
    }
  }
}
function getTreeRoute (routes, path) {
  if (!routes || routes.length === 0) return null
  let res = routes.find(item => item.path === path)
  if (!res) {
    const children = []
    routes.forEach(item => {
      item.children && item.children.length > 0 && children.push.apply(children, item.children)
    })
    res = getTreeRoute(children, path)
  }
  return res
}