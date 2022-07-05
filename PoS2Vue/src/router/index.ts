import { createRouter, createWebHistory } from 'vue-router'
import MyHomeView from '../views/MyHomeView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: MyHomeView
    },
    {
      path: '/unit',
      name: 'unit',
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('../views/UnitView.vue')
    },
    {
      path: '/item',
      name: 'item',
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('../views/ItemView.vue')
    },
    {
      path: '/receipt',
      name: 'receipt',
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('../views/ReceiptView.vue')
    },
    {
      path: '/pos',
      name: 'pos',
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('../views/PosView.vue')
    },
  ]
})

export default router
