import Dictionary from './internal/dictionary'

/**
 * 插件管理
 *
 * 说明：
 * 1 插件文件名必须与类名相同，否则可能引用不到
 * 2 插件可以有返回值，也可以没有返回值，譬如:全局混入
 */
export default class PluginManager {
  constructor() {
    this.plugins = new Dictionary()
  }

  add(type, instance) {
    this.plugins.add(type, instance)
  }

  count() {
    return this.plugins.count()
  }

  resolve(type) {
    let typeName = ''
    if (typeof type === 'string') {
      typeName = type
    } else {
      if (type.name) {
        typeName = type.name
      } else {
        console.error('the type is not found', type)
      }
    }

    return this.plugins.find(typeName)
  }

  instance(type) {
    const Class1 = this.resolve(type)
    if (typeof (Class1) === 'function') {
      return new Class1()
    } else {
      return Class1
    }
  }
}
