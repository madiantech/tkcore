import popup from '../../utils/popup'
import actionsheet from './index.vue'
let tkActionsheet = popup(actionsheet)

// const install = function (Vue) {
//   Vue.directive('tkActionsheet', tkActionsheet)
// }

// if (window.Vue) {
//   window.tkActionsheet = tkActionsheet
//   Vue.use(install); // eslint-disable-line
// }

// tkActionsheet.install = install
export default tkActionsheet