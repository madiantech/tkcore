import URI from 'urijs'
export function getQueryStringArgs (url) {
  // const u = url.split('?')
  // const qs = u.length > 1 ? u[1] : ''
  // let args = {}
  // if (qs) {
  //   qs.split('&').forEach(str => {
  //     const item = str.split('=')
  //     const key = decodeURIComponent(item[0])
  //     const value = decodeURIComponent(item[1])
  //     args[key] = value
  //   })
  // }
  // return args
  return URI(url).search(true)
}
export function getTempleteQuery (text, row) {
  const regex = /\*(.+?)\*/g; // *Id*->row.Id
  const options = text.match(regex)
  if (options && options.length > 0) {
    options.forEach(str => {
      const key = str.substring(1, str.length - 1)
      text = text.replace(str, row[key])
    })
  }
  return text
}

export function deepClearObject (data) {
  let obj = Object.assign({}, data)
  Object.keys(obj).forEach(key => {
    if (Object.prototype.toString.call(obj[key]) === '[object Array]') { //数组
      obj[key] = []
    } else {
      switch (typeof obj[key]) {
        case 'object': //对象
          obj[key] = deepClearObject(obj[key])
          break
        case 'number': //数字
          obj[key] = -1
          break
        case 'string':
          obj[key] = ''
          break
      }
    }
  })
  return obj
}

export function isEmptyObject (obj) {
  return typeof obj === 'object' && Object.keys(obj).length === 0
}

export function replaceUriSegment (tempUrl, key, i = 2) {
  let url = URI(tempUrl)
  let segment = url.segment()
  segment[i] = key
  url.segment(segment)
  return url.toString()
}