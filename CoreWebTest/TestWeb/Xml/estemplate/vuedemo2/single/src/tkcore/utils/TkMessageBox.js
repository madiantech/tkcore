import { MessageBox } from 'element-ui'

const DEFAULT_OPTIONS = {
  lockScroll: true,
  closeOnClickModal: false,
  closeOnPressEscape: true,
  confirmButtonText: '确定',
  cancelButtonText: '取消'
}

function _mergeDefaultOptions(options) {
  const opts = DEFAULT_OPTIONS

  if (options) {
    for (const p in options) {
      opts[p] = options[p]
    }
  }

  return opts
}

/**
 * 简易消息对话框
 *
 * see https://element.eleme.cn/#/zh-CN/component/message-box
 */
class TkMessageBox {
  alert(message, title = '警告', options) {
    const opts = _mergeDefaultOptions(options)
    return MessageBox.alert(message, title, opts)
  }

  info(message, title = '提示', options) {
    const opts = _mergeDefaultOptions(options)
    opts.type = 'info'

    return MessageBox.alert(message, title, opts)
  }

  success(message, title = '成功', options) {
    const opts = _mergeDefaultOptions(options)
    opts.type = 'success'
    return MessageBox.alert(message, title, opts)
  }

  warn(message, title = '警告', options) {
    const opts = _mergeDefaultOptions(options)
    opts.type = 'warning'
    return MessageBox.alert(message, title, opts)
  }

  error(message, title = '错误', options) {
    const opts = _mergeDefaultOptions(options)
    opts.type = 'error'
    return MessageBox.alert(message, title, opts)
  }

  confirm(message, title = '确认', options) {
    const opts = _mergeDefaultOptions(options)
    if (!opts.type) {
      opts.type = 'info'
    }
    return MessageBox.confirm(message, title, opts)
  }

  prompt(message, title, options) {
    const opts = _mergeDefaultOptions(options)
    return MessageBox.prompt(message, title, opts)
  }
}

const instance = new TkMessageBox()
export default instance
