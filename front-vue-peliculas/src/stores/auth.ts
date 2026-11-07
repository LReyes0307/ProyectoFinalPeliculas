import { computed, ref } from 'vue'
import { defineStore } from 'pinia'
import { http } from '../api/http'

interface LoginResponse {
  access_token: string
}

export const useAuthStore = defineStore('auth', () => {
  const token = ref<string | null>(localStorage.getItem('access_token'))
  const isAuthenticated = computed(() => Boolean(token.value))

  async function login(username: string, password: string) {
    const { data } = await http.post<LoginResponse>('/auth/login', {
      username,
      password,
    })

    token.value = data.access_token
    localStorage.setItem('access_token', data.access_token)
  }

  async function register(username: string, password: string) {
    await http.post('/auth/register', {
      username,
      password,
    })
  }

  function logout() {
    token.value = null
    localStorage.removeItem('access_token')
  }

  return { token, isAuthenticated, login, register, logout }
})
