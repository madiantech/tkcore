import axios from 'axios'
import { Notification } from 'element-ui';
// 创建axios实例
const service = axios.create()

// request拦截器
service.interceptors.request.use(
  config => {
    if (config.method === 'post') {
      config.headers['Content-Type'] = 'text/plain' // 关键所在
    }
    return config
  },
  error => {
    console.log(error) // for debug
    Promise.reject(error)
  }
)
// response 拦截器
service.interceptors.response.use(
  response => {
    const res = response.data
    if (typeof res === 'string' && res.indexOf('<html>') > -1) {
      Notification.error({
        title: '错误',
        dangerouslyUseHTMLString: true,
        message: `<strong style="word-break:break-all;">${response.config.url}</strong> 接口错误或不存在`
      })
      return Promise.reject(res)
    }
    if (res.Result && res.Result.Result === 'Fail') {
      Notification.error({
        title: '错误',
        dangerouslyUseHTMLString: true,
        message: `<strong style="word-break:break-all;">${response.config.url}</strong> 请求失败<a target="_blank" style="color:#F56C6C;" href="${res.Result.Message}">查看日志</a>`
      })
      return Promise.reject(res)
    } else {
      return res
    }
  },
  error => {
    console.log('err' + error) // for debug
    Notification.error({
      title: '错误',
      message: error
    })
    return Promise.reject(error)
  }
)
export default service