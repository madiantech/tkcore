/**
 * 内部帮助函数
 */
class Helpers {
  // 防抖
  Debounce(fn, t) {
    const delay = t || 500
    let timer

    return function() {
      const args = arguments
      if (timer) {
        clearTimeout(timer)
      }
      timer = setTimeout(() => {
        timer = null
        fn.apply(this, args)
      }, delay)
    }
  }

  // 节流
  Throttle(fn, t) {
    let last
    let timer
    const interval = t || 500
    return function() {
      const args = arguments
      const now = +new Date()
      if (last && now - last < interval) {
        clearTimeout(timer)
        timer = setTimeout(() => {
          last = now
          fn.apply(this, args)
        }, interval)
      } else {
        last = now
        fn.apply(this, args)
      }
    }
  }

  /**
   * 把对象b的扩展到对象a
   * @param a
   * @param b
   * @returns {*}
   */
  extend(a, b) {
    const o = Object.getOwnPropertyNames(b)
    o.forEach(attr => {
      a[attr] = b[attr]
    })
    return a
  }

  /**
   * 把指定对象的键值对扩展到网址
   * @param href
   * @param params
   * @returns {string|*}
   */
  addQueryString(href, params) {
    if (!params) { return href }
    // console.log(href)

    if (!href) {
      return ''
    }

    // 这里添加localhost是因为相对路径必须设置base
    // URL官方文档介绍，如果href是绝对路径，这base不起作用
    const url = new URL(href, 'http://localhost')
    for (const p in params) {
      url.searchParams.set(p, params[p])
    }
    // console.log(url)

    if (href.startsWith('http')) {
      return url.href
    } else {
      return url.pathname + url.search
    }
  }

  /**
   * 从url提取Querystring
   * @param url
   */
  getQueryStrings(url) {
    const querystrings = {}

    if (url == null || url === '' || url === undefined) {
      return querystrings
    }

    const arr1 = url.split('?')

    if (arr1.length !== 2) {
      return querystrings
    }

    const search = arr1[1]
    if (search == null || search === '' || search === undefined) {
      return querystrings
    }
    const arr2 = search.split('&')

    let arr3 = []
    for (const item in arr2) {
      arr3 = arr2[item].split('=')

      if (arr3.length === 2) {
        querystrings[arr3[0]] = arr3[1]
      }
    }

    return querystrings
  }
}

const helpers = new Helpers()
export default helpers
