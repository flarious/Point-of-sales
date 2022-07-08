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
      component: () => import('../views/UnitView.vue')
    },
    {
      path: '/item',
      name: 'item',
      component: () => import('../views/ItemView.vue')
    },
    {
      path: '/receipt',
      name: 'receipt',
      component: () => import('../views/ReceiptView.vue')
    },
    {
      path: '/pos',
      name: 'pos',
      component: () => import('../views/PosView.vue')
    },
  ]
})

export default router
