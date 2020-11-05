import Vue from 'vue'
import PluginManager from './plugin-manager'

const manager = new PluginManager()

// Load store modules dynamically.
const requireContext = require.context('./modules/', true, /.*\.js$/)

const modules = requireContext.keys()
  .map(file => [file.replace(/(^.\/)|(.js$)/g, ''), requireContext(file)])
  .reduce((modules, [name, module]) => {
    if (module.namespaced === undefined) {
      module.namespaced = true
    }

    return { ...modules,
      [name]: module
    }
  }, {})

for (const module in modules) {
  if (modules[module].default) {
    manager.add(module, modules[module].default)
  } else {
    manager.add(module, 'this plugin can not create an instance,it has been imported, ')
  }
}

Vue.prototype.$pluginManager = manager
