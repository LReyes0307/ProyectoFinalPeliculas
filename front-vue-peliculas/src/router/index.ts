import { createRouter, createWebHistory } from 'vue-router'
import LoginView from '../views/LoginView.vue'
import RegisterView from '../views/RegisterView.vue'
import { useAuthStore } from '../stores/auth'

export const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/',
      redirect: '/login',
    },
    {
      path: '/login',
      component: LoginView,
      meta: { guestOnly: true },
    },
    {
      path: '/register',
      component: RegisterView,
      meta: { guestOnly: true },
    },
    {
      path: '/movies',
      component: () => import('../views/MoviesView.vue'),
      meta: { requiresAuth: true },
    },
    {
      path: '/movies/create',
      component: () => import('../views/CreateMovieView.vue'),
      meta: { requiresAuth: true },
    },
    {
      path: '/movies/:id',
      component: () => import('../views/MovieDetailView.vue'),
      meta: { requiresAuth: true },
    },
    {
      path: '/movies/:id/edit',
      component: () => import('../views/CreateMovieView.vue'),
      meta: { requiresAuth: true },
    },
  ],
})

router.beforeEach((to) => {
  const authStore = useAuthStore()

  if (to.meta.requiresAuth && !authStore.isAuthenticated) {
    return '/login'
  }

  if (to.meta.guestOnly && authStore.isAuthenticated) {
    return '/movies'
  }
})
