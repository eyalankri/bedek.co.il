import Vue from 'vue'
import Router from 'vue-router'

import root from '@/components/root';
import Login from '@/components/account/login';
import Error404 from '@/components/error404';

import Buildings from '@/components/building/buildings';
import UpdateBuilding from '@/components/building/update-building';

import showApartment from '@/components/apartment/show';
import addUpdateApartment from '@/components/apartment/add-update';
import listApartment from '@/components/apartment/list';
import apartmentDocs from '@/components/apartment/docs';

import handyman from '@/components/handyman/main';
import addUpdateHandyman from '@/components/handyman/add-update';
import serviceInHandyman from '@/components/ServiceInHandyman/list';
import handymanInBuilding from '@/components/HandymanInBuilding/list';

import serviceList from '@/components/service/list';
import addUpdateService from '@/components/service/add-update';

import serviceCall from '@/components/serviceCall/main';
import serviceCallList from '@/components/serviceCall/list';



import axios from "axios";
Vue.use(Router)

const router = new Router({
  // this time we stored Router() in a const var and we can use it below
  routes: [
    {
      path: '/',
      name: 'root',
      component: root,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/login',
      name: 'Login',
      component: Login
    },
    {
      path: '/service-call-list/',
      name: 'serviceCallList',
      component: serviceCallList,
      props: true,
      params: null,
      meta: { requiresAuth: true }
    },
    {
      path: '/service-call/:apartmentId/:serviceCallId?',
      name: 'serviceCall',
      component: serviceCall,
      props: true,
      params: null,
      meta: { requiresAuth: true },
     
    },
    {
      path: '/service/list',
      name: 'serviceList',
      component: serviceList,
      props: true,
      params: null,
      meta: { requiresAuth: true }
    },
    {
      //http://localhost:8080/#/add-update/1
      path: '/service/add-update/:serviceId?',
      name: 'addUpdateService',
      component: addUpdateService,
      props: true,
      params: null,
      meta: { requiresAuth: true }
    },
    {
      path: '/handyman/main',
      name: 'handyman',
      component: handyman,
      props: true,
      params: null,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/handyman/add-update/:handymanId?',
      name: 'addUpdateHandyman',
      component: addUpdateHandyman,
      props: true,
      params: null,
      meta: {
        requiresAuth: true
      }
    },
    {
      //http://localhost:8080/#/service-in-handyman/list/d2ea01e8-0f9f-4bc2-a3a8-9e2e5ce0071e
      path: '/service-in-handyman/list/:handymanId?',
      name: 'serviceInHandyman',
      component: serviceInHandyman,
      props: true,
      params: null,
      meta: {
        requiresAuth: true
      }
    },
    {
      //http://localhost:8080/#/handyman-in-building/list/2/d2ea01e8-0f9f-4bc2-a3a8-9e2e5ce0071e
      path: '/handyman-in-building/list/:buildingId',
      name: 'handymanInBuilding',
      component: handymanInBuilding,
      props: true,
      params: null,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/buildings',
      name: 'buildings',
      component: Buildings,
      props: true,
      params: null,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/update-building/:buildingId',
      name: 'updateBuilding',
      component: UpdateBuilding,
      props: true,
      params: null,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/apartment/docs/:apartmentId',
      name: 'apartmentDocs',
      component: apartmentDocs,
      props: true,
      params: null,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/apartment/add-update/:id', // id could be appartmentId Or bulidingId
      name: 'addUpdateApartment',
      component: addUpdateApartment,
      props: true,
      params: null,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/apartment/list/:id', // id could be appartmentId Or bulidingId
      name: 'listApartment',
      component: listApartment,
      props: true,
      params: null,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: '/apartment/show/:id', // id could be appartmentId Or bulidingId
      name: 'showApartment',
      component: showApartment,
      props: true,
      params: null,
      meta: {
        requiresAuth: true
      }
    },

    {
      path: '*',
      name: 'Error404',
      component: Error404
    }

  ]
});


router.beforeEach((to, form, next) => {


  // check to see if route requires auth
  if (to.matched.some(rec => rec.meta.requiresAuth)) {

    let isTokenValid = false;
    let token = localStorage.getItem("user-token");

    var headers = {
      headers: {
        'Authorization': "Bearer " + token
      }
    };
    axios
      .get(process.env.ROOT_API + "/Account/IsTokenValid", headers)
      .then(response => {

        next(); // token will be valid

      })
      .catch(error => {
        localStorage.removeItem('user-token');
        next({ name: 'Login' });
      });;



  }
  else { // does not need auth
    next()
  }
});

export default router // we export it here becuase we use route as variable