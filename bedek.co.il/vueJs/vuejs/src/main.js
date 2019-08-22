// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import App from './App'
import router from './router'
import { store } from './store/store'
import VueJsModal from 'vue-js-modal'

Vue.config.productionTip = false

/* eslint-disable no-new */


Vue.use(VueJsModal,)

new Vue({
  store: store,
  el: '#app',
  router,
  components: { App },
  template: '<App/>'
})

