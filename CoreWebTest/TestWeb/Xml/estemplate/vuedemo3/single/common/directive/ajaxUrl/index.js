export default function (el, binding, vnode) {
  el.onclick = function () {
    const value = binding.value
    if (!value) return
    // 处理获取的值
    let url, confirm, method = 'get'
    if (typeof value === 'string') {
      url = value
    } else if (typeof value === 'object') {
      url = value.url
      confirm = value.confirm
      method = value.method || 'get'
      if (!url) return
    } else {
      return
    }

    const parentVm = vnode.context
    // 有confirm
    if (confirm) {
      parentVm.$confirm(confirm, '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消'
      }).then(() => {
        ajaxFunction()
      }).catch(() => {

      })
    } else {
      ajaxFunction()
    }
    function ajaxFunction () {
      parentVm.$axios({
        method,
        url
      }).then(res => {
        parentVm.$emit('ajax-success')
      }).catch(err => {
        parentVm.$emit('ajax-error')
      })
    }
  }
}
