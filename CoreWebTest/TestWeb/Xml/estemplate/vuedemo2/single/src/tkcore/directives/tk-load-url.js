import Vue from 'vue'
import TkDebug from '../utils/TkDebug.js'
import TkMessageBox from '../utils/TkMessageBox.js'
import TkParamParser from '../utils/TkParamParser'

/*
附着标签：a，button
主要作用：将url的内容自动加载到div中
参数：string | {url：string，[parseSource:object]}
参数含义：
  url 需要加载内容的url地址
*/

Vue.directive('tkLoadUrl', {

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
    let parseSource = null
    if (typeof (binding.value) === 'string') {
      url = binding.value
    } else {
      url = binding.value.url !== undefined ? binding.value.url : ''
      parseSource = binding.value.parseSource !== undefined ? binding.value.parseSource : null
    }

    if (parseSource) {
      url = TkParamParser.parseWithDataRow(parseSource, url)
    }

    el.onclick = async function(e) {
      if (!url) {
        TkDebug.debug('tk-load-url.js', 'v-tk-load-url设置格式错误', false)
        return
      }

      // TODO tk-ajax-url asscess api
    }
  }

})
