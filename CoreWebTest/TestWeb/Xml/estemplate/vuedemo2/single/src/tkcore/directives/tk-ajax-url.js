import Vue from 'vue'
import TkDebug from '../utils/TkDebug'
import TkMessageBox from '../utils/TkMessageBox'
import request from '../utils/request'
import TkParamParser from '../utils/TkParamParser'

/*
附着标签：a，button
主要作用：ajax操作url
参数：string | {url：string，[confirm：string，method：string,parseSource:object]}
参数含义：
  url url是跳转的地址，如果没有其他参数，直接写就行
  confirm 如果配置，则在跳转前进行询问，得到yes回答才能跳转，
  method method为http的方式，默认为get
*/

Vue.directive('tkAjaxUrl', {

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
    let method = 'get'
    let parseSource = null

    if (typeof (binding.value) === 'string') {
      url = binding.value
    } else {
      url = binding.value.url !== undefined ? binding.value.url : ''
      confirm = binding.value.confirm !== undefined ? binding.value.confirm : ''
      method = binding.value.method !== undefined ? binding.value.method : 'get'
      parseSource = binding.value.parseSource !== undefined ? binding.value.parseSource : null
    }

    if (parseSource) {
      url = TkParamParser.parseWithDataRow(parseSource, url)
    }

    el.onclick = async function(e) {
      if (!url) {
        TkDebug.debug('tk-ajax-url.js', 'v-tk-ajax-url设置格式错误', false)
        return
      }

      // 确认
      if (confirm) {
        const res = await TkMessageBox.confirm(confirm)
        if (res !== 'confirm') {
          return
        }
      }

      const config = {
        url: url,
        method: method
      }
      request(config)
        .then(response => {
          // TODO tk-ajax-url asscess api
        })
        .catch(error => {
          TkMessageBox.error('数据提交失败！请稍候重试。')
          TkDebug.debug('tk-ajax-url.js', error)
        })
        .then(() => {
        })
    }
  }

})
