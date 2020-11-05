import TkParamParser from './TkParamParser'
import TkMessageBox from './TkMessageBox'

class Utils {
  JumpUrl(vue, url, confirm, newwindow, withRetUrl, parseSource) {
    if (!url) {
      return
    }

    if (parseSource) {
      url = TkParamParser.parseWithDataRow(parseSource, url)
    }

    // 确认
    if (confirm) {
      TkMessageBox.confirm(confirm).then(res => {
        if (res !== 'confirm') {
          return
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
          const route = vue.$router.resolve(url)
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
              vue.$router.push(route.location)
            }
          }
        }
      })
    }
  }
}

const instance = new Utils()
export default instance
