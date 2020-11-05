import popup from '../../utils/popup'
import drawer from './index.vue'
let tkDrawer = popup(drawer)


// const install = function (Vue) {
//   Vue.directive('tkDialog', tkDialog)
// }

// if (window.Vue) {
//   window.tkDialog = tkDialog
//   Vue.use(install); // eslint-disable-line
// }

// tkDialog.install = install
export default tkDrawer
