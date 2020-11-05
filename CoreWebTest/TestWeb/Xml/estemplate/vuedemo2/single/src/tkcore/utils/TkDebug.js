import TkMessageBox from './TkMessageBox.js'

/**
 * 调试信息打印类
 */
class TkDebug {
  debug(where, info, silent = true) {
    console.log(where, info)
    if (!silent && typeof (info) === 'string') {
      TkMessageBox.error(info, '错误', {})
    }
  }
}

const instance = new TkDebug()
export default instance
