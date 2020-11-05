import Vue from 'vue'
import TkDebug from '../utils/TkDebug.js'
import TkMessageBox from '../utils/TkMessageBox.js'
import TkParamParser from '../utils/TkParamParser'

/*
附着标签：a，button
主要作用：跳转到指定页面，通常情况下，需要在跳转的url上自动添加RetUrl={转义后的本页url地址}
参数：string | {url：string，[confirm：string， newwindow：bool， withRetUrl：bool,parseSource:object]}
参数含义：
  url 是对话框显示内容的地址，如果没有其他参数，直接写就行；
  confirm 如果配置，则在跳转前进行询问，得到yes回答才能跳转，
  newwindow newwindow为true时，打开新的页面进行跳转，默认为false
  withRetUrl withRetUrl为true时，需要再url自动添加RetUrl的参数，默认为false
*/

Vue.directive('tkUrl', {

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
    let confirm = ''
    let newwindow = false
    let withRetUrl = false
    let parseSource = null

    if (typeof (binding.value) === 'string') {
      url = binding.value
    } else {
      url = binding.value.url !== undefined ? binding.value.url : ''
      confirm = binding.value.confirm !== undefined ? binding.value.confirm : ''
      newwindow = binding.value.newwindow !== undefined ? binding.value.newwindow : false
      withRetUrl = binding.value.withRetUrl !== undefined ? binding.value.withRetUrl : true
      parseSource = binding.value.parseSource !== undefined ? binding.value.parseSource : null
    }

    if (parseSource) {
      url = TkParamParser.parseWithDataRow(parseSource, url)
    }

    el.onclick = async function(e) {
      if (!url) {
        TkDebug.debug('tk-url.js', 'v-tk-url设置格式错误', false)
        return
      }

      // 确认
      if (confirm) {
        const res = await TkMessageBox.confirm(confirm)
        if (res !== 'confirm') {
          return
        }
      }

      if (url.startsWith('http')) { // start http or https
        let jumpUrl = url

        // withRetUrl
        if (withRetUrl) {
          const url = new URL(jumpUrl)
          url.searchParams.set('RetUrl', encodeURI(location.href))
          jumpUrl = url
        }
        // console.log(jumpUrl);
        if (newwindow) {
          window.open(jumpUrl)
        } else {
          location.href = jumpUrl
        }
      } else { // vue路由
        const route = vnode.context.$router.resolve(url)
        // console.log(route)
        if (route) {
          if (newwindow) {
            let jumpUrl = route.href

            // withRetUrl
            if (withRetUrl) {
              const url = new URL(jumpUrl)
              url.searchParams.set('RetUrl', encodeURI(location.href))
              jumpUrl = url
            }
            window.open(jumpUrl)
          } else {
            route.location.query.RetUrl = encodeURIComponent(location.href)
            vnode.context.$router.push(route.location)
          }
        } else {
          TkDebug.debug('tk-url.js', '设置的url找不到路由', false)
        }
      }
    }
  }

})
