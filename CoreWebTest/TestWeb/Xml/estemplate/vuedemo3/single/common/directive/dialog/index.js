import popup from '../../utils/popup'
import dialog from './index.vue'
let tkDialog = popup(dialog)


// const install = function (Vue) {
//   Vue.directive('tkDialog', tkDialog)
// }

// if (window.Vue) {
//   window.tkDialog = tkDialog
//   Vue.use(install); // eslint-disable-line
// }

// tkDialog.install = install
export default tkDialog
