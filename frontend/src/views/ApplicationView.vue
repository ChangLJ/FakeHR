<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import { Motion } from 'motion-v'
import { LogOut, FileText, Save, ClipboardList } from 'lucide-vue-next'
import { useAuthStore } from '@/stores/auth'
import { useApplicationStore } from '@/stores/application'
import Tabs from '@/components/ui/Tabs.vue'
import Button from '@/components/ui/Button.vue'
import BasicInfoTab from '@/views/tabs/BasicInfoTab.vue'
import WorkHistoryTab from '@/views/tabs/WorkHistoryTab.vue'
import SelfIntroTab from '@/views/tabs/SelfIntroTab.vue'

const router = useRouter()
const auth = useAuthStore()
const appStore = useApplicationStore()
const activeTab = ref('basic')

const tabs = [
  { id: 'basic', label: '基本資料' },
  { id: 'work', label: '工作經歷' },
  { id: 'intro', label: '自我簡介' },
]

onMounted(() => appStore.fetchApplication())

async function logout() {
  await auth.logout()
  router.push('/login')
}

async function saveAll() {
  if (!confirm('確定儲存整份申請表？')) return
  await appStore.saveApplication()
}
</script>

<template>
  <div class="min-h-screen bg-slate-50">
    <header class="border-b border-border bg-white shadow-sm">
      <div class="mx-auto flex max-w-6xl items-center justify-between px-4 py-4">
        <div class="flex items-center gap-3">
          <FileText class="h-8 w-8 text-primary" />
          <div>
            <h1 class="text-xl font-bold text-slate-800">工作申請表</h1>
            <p class="text-sm text-muted-foreground">{{ auth.chineseName || auth.idNumber }}</p>
          </div>
        </div>
        <div class="flex items-center gap-2">
          <Button variant="outline" @click="router.push('/review')">
            <ClipboardList class="mr-2 h-4 w-4" /> 審閱履歷
          </Button>
          <Button size="lg" :disabled="appStore.saving || appStore.loading" @click="saveAll">
            <Save class="mr-2 h-4 w-4" />
            {{ appStore.saving ? '儲存中...' : '儲存申請表' }}
          </Button>
          <Button variant="outline" @click="logout">
            <LogOut class="mr-2 h-4 w-4" /> 登出
          </Button>
        </div>
      </div>
    </header>

    <main class="mx-auto max-w-6xl px-4 py-6">
      <p class="mb-2 text-sm text-muted-foreground">可自由切換頁籤編輯，完成後請按上方「儲存申請表」一次儲存全部內容。</p>
      <p class="mb-4 text-sm text-destructive">※ 標示欄位為必填</p>
      <p v-if="appStore.draftRestored" class="mb-4 text-center text-xs text-amber-600">已從本機暫存還原未送出的資料</p>

      <Tabs v-model="activeTab" :tabs="tabs" class="mb-6" />

      <Motion
        :key="activeTab"
        :initial="{ opacity: 0, x: 8 }"
        :animate="{ opacity: 1, x: 0 }"
        :transition="{ duration: 0.25 }"
      >
        <div v-if="appStore.loading" class="py-12 text-center text-muted-foreground">載入中...</div>
        <template v-else>
          <BasicInfoTab v-show="activeTab === 'basic'" />
          <WorkHistoryTab v-show="activeTab === 'work'" />
          <SelfIntroTab v-show="activeTab === 'intro'" />
        </template>
      </Motion>

      <p v-if="appStore.validationError" class="mt-4 text-center text-sm text-destructive">{{ appStore.validationError }}</p>
      <p v-if="appStore.message" class="mt-4 text-center text-sm font-medium text-primary">{{ appStore.message }}</p>
      <p v-if="appStore.error" class="mt-2 text-center text-sm text-destructive">{{ appStore.error }}</p>

      <div v-if="!appStore.loading" class="mt-6 flex justify-center">
        <Button size="lg" :disabled="appStore.saving" @click="saveAll">
          <Save class="mr-2 h-4 w-4" />
          {{ appStore.saving ? '儲存中...' : '儲存申請表' }}
        </Button>
      </div>
    </main>
  </div>
</template>
