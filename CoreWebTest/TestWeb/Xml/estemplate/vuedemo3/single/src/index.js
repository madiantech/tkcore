import Vue from 'vue'
import ElementUI from 'element-ui';
import 'element-ui/lib/theme-chalk/index.css';
import directives from 'common/directive'
import 'common/css/index.css'
import 'common/css/font-awesome/css/font-awesome.min.css'
import App from './app.vue'
import router from './router'
import 'common/components'
import config from './config'
import request from 'common/utils/request'
Vue.use(ElementUI)
Object.keys(directives).forEach(key => {
  Vue.directive(key, directives[key])
})
Vue.prototype.$config = config
Vue.prototype.$axios = request
new Vue({
  el: '#app',
  router,
  template: '<App/>',
  components: {
    App
  }
});
