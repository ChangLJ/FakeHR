import { defineStore } from 'pinia'
import { ref } from 'vue'
import api from '@/api/client'

export type ApplicationSummary = {
  id: string
  idNumber: string
  chineseName?: string
  englishName?: string
  recruitPosition?: string
  email?: string
  updatedAt: string
}

export const useReviewStore = defineStore('review', () => {
  const list = ref<ApplicationSummary[]>([])
  const loading = ref(false)
  const detail = ref<Record<string, unknown> | null>(null)
  const detailLoading = ref(false)

  async function fetchList() {
    loading.value = true
    try {
      const { data } = await api.get<ApplicationSummary[]>('/review/applications')
      list.value = data
    } finally {
      loading.value = false
    }
  }

  async function fetchDetail(id: string) {
    detailLoading.value = true
    detail.value = null
    try {
      const { data } = await api.get(`/review/applications/${id}`)
      detail.value = data as Record<string, unknown>
    } finally {
      detailLoading.value = false
    }
  }

  function clearDetail() {
    detail.value = null
  }

  return { list, loading, detail, detailLoading, fetchList, fetchDetail, clearDetail }
})
