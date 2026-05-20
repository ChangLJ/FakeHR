<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { Motion } from 'motion-v'
import { LogIn, UserPlus } from 'lucide-vue-next'
import { useAuthStore } from '@/stores/auth'
import Card from '@/components/ui/Card.vue'
import Button from '@/components/ui/Button.vue'
import Input from '@/components/ui/Input.vue'
import FormField from '@/components/FormField.vue'

const router = useRouter()
const auth = useAuthStore()
const mode = ref<'login' | 'register'>('login')
const idNumber = ref('D123456789')
const password = ref('demo1234')
const chineseName = ref('張力仁')
const englishName = ref('Louis')
const error = ref('')
const loading = ref(false)

async function submit() {
  error.value = ''
  loading.value = true
  try {
    if (mode.value === 'login') {
      await auth.login(idNumber.value, password.value)
    } else {
      await auth.register({ idNumber: idNumber.value, password: password.value, chineseName: chineseName.value, englishName: englishName.value })
    }
    router.push('/')
  } catch {
    error.value = mode.value === 'login' ? '登入失敗，請確認帳密' : '註冊失敗，身分證號可能已存在'
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <div class="flex min-h-screen items-center justify-center bg-gradient-to-br from-cyan-50 to-slate-100 p-4">
    <Motion
      :initial="{ opacity: 0, y: 20 }"
      :animate="{ opacity: 1, y: 0 }"
      :transition="{ duration: 0.4 }"
      class="w-full max-w-md"
    >
      <Card class="p-8">
        <div class="mb-6 text-center">
          <h1 class="text-2xl font-bold text-slate-800">工作申請表</h1>
          <p class="mt-1 text-sm text-muted-foreground">人力資源招募系統</p>
        </div>

        <div class="mb-4 flex gap-2">
          <Button :variant="mode === 'login' ? 'default' : 'outline'" class="flex-1" @click="mode = 'login'">
            <LogIn class="mr-2 h-4 w-4" /> 登入
          </Button>
          <Button :variant="mode === 'register' ? 'default' : 'outline'" class="flex-1" @click="mode = 'register'">
            <UserPlus class="mr-2 h-4 w-4" /> 註冊
          </Button>
        </div>

        <form class="space-y-4" @submit.prevent="submit">
          <FormField label="身分證/護照號" required>
            <Input v-model="idNumber" maxlength="20" required />
          </FormField>
          <FormField v-if="mode === 'register'" label="中文姓名" required>
            <Input v-model="chineseName" required />
          </FormField>
          <FormField v-if="mode === 'register'" label="英文姓名" required>
            <Input v-model="englishName" required />
          </FormField>
          <FormField label="密碼" required>
            <Input v-model="password" type="password" required />
          </FormField>
          <p v-if="error" class="text-sm text-destructive">{{ error }}</p>
          <Button type="submit" class="w-full" :disabled="loading">
            {{ loading ? '處理中...' : mode === 'login' ? '登入' : '註冊並登入' }}
          </Button>
          <p v-if="mode === 'login'" class="text-center text-xs text-muted-foreground">示範帳號：D123456789 / demo1234（假資料，非真實個資）</p>
        </form>
      </Card>
    </Motion>
  </div>
</template>
