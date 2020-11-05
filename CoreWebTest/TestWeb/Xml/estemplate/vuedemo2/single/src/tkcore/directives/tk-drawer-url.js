import Vue from 'vue'
import TkDebug from '../utils/TkDebug.js'
import TkParamParser from '../utils/TkParamParser'

/*
附着标签：a，button
主要作用：抽屉对话框，对话框中访问url，显示其返回内容
参数：string | {url：string，[title：string，height：int，width：int，useiFrame：bool， closeAction：string,parseSource:object]}
参数含义：
  url 是对话框显示内容的地址，如果没有其他参数，直接写就行；
  title 是对话框的标题，。
  width 为对话框的宽度，含义同width相同。
  useiFrame，当为true时，提供iFrame的方式显示url中的内容，此时url返回应该是全部HTML。
  closeAction 是关闭对话框后需要做的操作，设想以插件的方式处理，这里提供的是注册名。
  （另：该参数仅是设想，可能会根据对话框的API进行适当的调整）
*/

/**
 * Resolve async components.
 *
 * @param  {Array} components
 * @return {Array}
 */
async function resolveComponents(components) {
  return await Promise.all(components.map(async component => {
    return typeof component === 'function' ? await component() : component
  }))
}

Vue.directive('tkDrawerUrl', {

  /**
   * 指令所在组件的 VNode 及其子 VNode 全部更新后调用。
   *
   * @param {Object} el 指令所绑定的元素，可以用来直接操作 DOM。
   * @param {Object} binding
   *      name：指令名，不包括 v- 前缀。
   *      rawName:指令名，包含v-和修饰
   *      value：指令的绑定值，例如：v-my-directive="1 + 1" 中，绑定值为 2。
   *      expression：字符串形式的指令表达式。例如 v-my-directive="1 + 1" 中，表达式为 "1 + 1"。
   *      arg：传给指令的参数，可选。例如 v-my-directive:foo 中，参数为 "foo"。
   *      modifiers：一个包含修饰符的对象。例如：v-my-directive.foo.bar 中，修饰符对象为 { foo: true, bar: true }。
   * @param {Object} vnode  Vue 编译生成的虚拟节点。移步 VNode API 来了解更多详情。
   */
  componentUpdated: function(el, binding, vnode) {
    // console.log(binding.name+":bind");

    let url
    let title
    let height = 300
    let width = '40%'
    let parseSource = null
    let closeOnClick = false
      // const useiFrame = false;
      // let closeAction

    if (typeof (binding.value) === 'string') {
      url = binding.value
    } else {
      url = binding.value.url !== undefined ? binding.value.url : ''
      title = binding.value.title !== undefined ? binding.value.title : ''
      height = binding.value.height !== undefined ? binding.value.height : 300
      width = binding.value.width !== undefined ? binding.value.width : '40%'
      parseSource = binding.value.parseSource !== undefined ? binding.value.parseSource : null
      closeOnClick = binding.value.closeOnClick !== undefined ? binding.value.closeOnClick : false
      // useiFrame = binding.value.useiFrame?binding.value.useiFrame:false
      // closeAction = binding.value.closeAction !== undefined ? binding.value.closeAction : null
    }

    if (parseSource) {
      url = TkParamParser.parseWithDataRow(parseSource, url)
    }

    el.onclick = async function(e) {
      if (!url) {
        TkDebug.debug('tk-drawer-url.js', 'v-tk-drawer-url设置格式错误', false)
        return
      }

      vnode.context.drawerUrl = url
      vnode.context.drawerTitle = title
      vnode.context.drawerHeight = height
      vnode.context.drawerWidth = width
      vnode.context.drawerCloseOnClickModal = closeOnClick

      // TODO closeAction
      // vnode.context.drawerCloseAction = closeAction;

      if (url.startsWith('http')) { // start http or https
        vnode.context.drawerUseiFrame = true
      } else {
        vnode.context.drawerUseiFrame = false

        const components = await resolveComponents(
          vnode.context.$router.getMatchedComponents(url)
        )
        // console.log(components)
        if (components.length === 0) {
          TkDebug.debug('tk-drawer-url.js', '设置的url找不到路由', false)
          return
        } else if (components.length === 1) {
          // Vue.component('drawer-inner-form', components[0].default)
          TkDebug.debug('tk-drawer-url.js', '设置的url找不到路由', false)
          return
        } else {
          // 注意：这里根据加载机制，也许是Vue.component('drawer-inner-form', components[1].default)，暂未调查
          Vue.component('drawer-inner-form', components[1].default || components[1])
        }
      }
      vnode.context.drawerVisible = true
    }
  }

})
