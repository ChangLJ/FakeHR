import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import api from '@/api/client'

export const useAuthStore = defineStore('auth', () => {
  const token = ref(localStorage.getItem('access_token') || '')
  const userId = ref(localStorage.getItem('user_id') || '')
  const idNumber = ref(localStorage.getItem('id_number') || '')
  const chineseName = ref(localStorage.getItem('chinese_name') || '')

  const isAuthenticated = computed(() => !!token.value)

  function setSession(data: { accessToken: string; userId: string; idNumber: string; chineseName?: string }) {
    token.value = data.accessToken
    userId.value = data.userId
    idNumber.value = data.idNumber
    chineseName.value = data.chineseName || ''
    localStorage.setItem('access_token', data.accessToken)
    localStorage.setItem('user_id', data.userId)
    localStorage.setItem('id_number', data.idNumber)
    localStorage.setItem('chinese_name', data.chineseName || '')
  }

  async function login(id: string, password: string) {
    const { data } = await api.post('/auth/login', { idNumber: id, password })
    setSession({
      accessToken: data.accessToken,
      userId: data.userId,
      idNumber: data.idNumber,
      chineseName: data.chineseName,
    })
  }

  async function register(payload: { idNumber: string; password: string; chineseName: string; englishName: string }) {
    const { data } = await api.post('/auth/register', payload)
    setSession({
      accessToken: data.accessToken,
      userId: data.userId,
      idNumber: data.idNumber,
      chineseName: data.chineseName,
    })
  }

  async function logout() {
    try {
      await api.post('/auth/logout')
    } finally {
      token.value = ''
      userId.value = ''
      idNumber.value = ''
      chineseName.value = ''
      localStorage.clear()
    }
  }

  return { token, userId, idNumber, chineseName, isAuthenticated, login, register, logout }
})
