/**
 * 宏解析类
 *
 *  作用：把传入的字符串中*Id*这种格式的字符用，实际数据替换
 *  格式说明：
 *    *Id* - 从所在数据行获取值 支持*stu.id*这种格式获取子属性的值
 *    %Id% - 从querystring获取值 支持*stu.id*这种格式获取子属性的值
 */
class TkParamParser {
  /**
   * 从对象获知指定值
   * @param obj - 对象
   * @param key - 关键字
   * @returns {*}
   */
  getDeepValue(obj, key) {
    const arr = key.split('.')

    let ret = obj
    for (let i = 0; i <= arr.length - 1; i++) {
      ret = ret[arr[i]]
    }
    return ret
  }

  /**
   * 从数据行获取值
   * @param dataRow - 数据行
   * @param originString - 原字符串
   * @returns {string} - 替换后字符串
   */
  parseWithDataRow(dataRow, originString) {
    if (!originString) { return '' }

    const params = originString.match(/\*[[a-zA-Z0-9.]+\*/g)

    let resultStr = originString
    let key = ''
    for (const p in params) {
      key = params[p].substr(1, params[p].length - 2)
      // console.log(key)
      resultStr = resultStr.replace(params[p], this.getDeepValue(dataRow, key))
    }

    return resultStr
  }

  /**
   * 从querystrings获取值
   * @param querystrings
   * @param originString
   * @returns {string|*|void|string}
   */
  parseWithQueryStrings(querystrings, originString) {
    if (!originString) { return '' }

    const params = originString.match(/%[[a-zA-Z0-9.]+%/g)

    let resultStr = originString
    let key = ''
    for (const p in params) {
      key = params[p].substr(1, params[p].length - 2)
      // console.log(key)
      resultStr = resultStr.replace(params[p], this.getDeepValue(querystrings, key))
    }

    return resultStr
  }
}

const instance = new TkParamParser()
export default instance
