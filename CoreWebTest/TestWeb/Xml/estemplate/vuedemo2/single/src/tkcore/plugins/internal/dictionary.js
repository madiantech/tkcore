export default class Dictionary {
  constructor() {
    this.dataStore = []
  }

  // 向字典添加元素
  add(key, value) {
    this.dataStore[key] = value
  }

  // 查找字典中的元素
  find(key) {
    return this.dataStore[key]
  }

  // 删除一个元素
  remove(key) {
    if (this.dataStore[key]) {
      delete this.dataStore[key]
    }
  }

  // 显示字典元素
  print() {
    const sortKeys = Object.keys(this.dataStore).sort()
    for (const key in sortKeys) {
      console.log(sortKeys[key] + '->' + this.dataStore[sortKeys[key]])
    }
  }

  // 查看字典中元素的个数
  count() {
    let n = 0
    // eslint-disable-next-line
    for (const key in this.dataStore) {
      ++n
    }
    return n
  }

  // 清空字典

  clear() {
    for (const key in this.dataStore) {
      delete this.dataStore[key]
    }
  }
}
