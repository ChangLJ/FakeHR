<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import { ArrowLeft, Eye, FileText } from 'lucide-vue-next'
import { useReviewStore } from '@/stores/review'
import Button from '@/components/ui/Button.vue'
import Card from '@/components/ui/Card.vue'
import ApplicationDetailModal from '@/components/ApplicationDetailModal.vue'

const router = useRouter()
const review = useReviewStore()
const modalOpen = ref(false)
const selectedTitle = ref('')

onMounted(() => review.fetchList())

async function openResume(id: string, label: string) {
  selectedTitle.value = label
  modalOpen.value = true
  await review.fetchDetail(id)
}

function closeModal() {
  modalOpen.value = false
  review.clearDetail()
}
</script>

<template>
  <div class="min-h-screen bg-slate-50">
    <header class="border-b border-border bg-white shadow-sm">
      <div class="mx-auto flex max-w-5xl items-center justify-between px-4 py-4">
        <div class="flex items-center gap-3">
          <FileText class="h-8 w-8 text-primary" />
          <h1 class="text-xl font-bold text-slate-800">履歷審閱</h1>
        </div>
        <Button variant="outline" @click="router.push('/')">
          <ArrowLeft class="mr-2 h-4 w-4" /> 返回填寫
        </Button>
      </div>
    </header>

    <main class="mx-auto max-w-5xl px-4 py-6">
      <p class="mb-4 text-sm text-muted-foreground">選擇一份申請表以檢視完整內容。</p>

      <div v-if="review.loading" class="py-12 text-center text-muted-foreground">載入清單中...</div>

      <div v-else-if="!review.list.length" class="py-12 text-center text-muted-foreground">尚無申請資料</div>

      <div v-else class="space-y-3">
        <Card
          v-for="item in review.list"
          :key="item.id"
          class="flex flex-wrap items-center justify-between gap-3 p-4"
        >
          <div>
            <p class="font-medium">{{ item.chineseName || item.idNumber }}</p>
            <p class="text-sm text-muted-foreground">
              {{ item.idNumber }}
              <span v-if="item.recruitPosition"> · {{ item.recruitPosition }}</span>
            </p>
            <p class="text-xs text-muted-foreground">
              更新：{{ new Date(item.updatedAt).toLocaleString('zh-TW') }}
            </p>
          </div>
          <Button @click="openResume(item.id, `${item.chineseName || item.idNumber} 的履歷`)">
            <Eye class="mr-2 h-4 w-4" /> 檢視
          </Button>
        </Card>
      </div>
    </main>

    <ApplicationDetailModal
      :open="modalOpen"
      :loading="review.detailLoading"
      :data="review.detail"
      :title="selectedTitle"
      @close="closeModal"
    />
  </div>
</template>
