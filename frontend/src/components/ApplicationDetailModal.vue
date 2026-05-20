<script setup lang="ts">
import { computed } from 'vue'
import { X } from 'lucide-vue-next'
import Button from '@/components/ui/Button.vue'

const props = defineProps<{
  open: boolean
  loading?: boolean
  data: Record<string, unknown> | null
  title?: string
}>()

const emit = defineEmits<{ close: [] }>()

const basic = computed(() => props.data?.basic as Record<string, unknown> | undefined)
const work = computed(() => props.data?.work as Record<string, unknown> | undefined)
const intro = computed(() => props.data?.selfIntro as Record<string, unknown> | undefined)

function salaryLabel() {
  const s = intro.value
  if (!s) return '-'
  if (s.salaryByCompanyRule) return '依公司規定'
  if (s.salaryNegotiable) return '面議'
  const parts = []
  if (s.expectedMonthlySalary) parts.push(`月薪 ${s.expectedMonthlySalary}`)
  if (s.expectedYearlySalary) parts.push(`年薪 ${s.expectedYearlySalary}`)
  return parts.length ? parts.join(' / ') : '-'
}
</script>

<template>
  <Teleport to="body">
    <div
      v-if="open"
      class="fixed inset-0 z-50 flex items-center justify-center bg-black/50 p-4"
      @click.self="emit('close')"
    >
      <div class="flex max-h-[90vh] w-full max-w-4xl flex-col rounded-lg bg-white shadow-xl">
        <div class="flex items-center justify-between border-b px-6 py-4">
          <h2 class="text-lg font-semibold">{{ title || '履歷內容' }}</h2>
          <Button variant="ghost" size="sm" @click="emit('close')"><X class="h-5 w-5" /></Button>
        </div>

        <div class="overflow-y-auto px-6 py-4">
          <div v-if="loading" class="py-12 text-center text-muted-foreground">載入中...</div>
          <template v-else-if="data">
            <section class="mb-6">
              <h3 class="mb-2 font-medium text-primary">基本資料</h3>
              <dl class="grid gap-2 text-sm sm:grid-cols-2">
                <div><dt class="text-muted-foreground">姓名</dt><dd>{{ basic?.chineseName }} ({{ basic?.englishName }})</dd></div>
                <div><dt class="text-muted-foreground">身分證號</dt><dd>{{ basic?.idNumber }}</dd></div>
                <div><dt class="text-muted-foreground">應徵項目</dt><dd>{{ basic?.recruitPosition || '-' }}</dd></div>
                <div><dt class="text-muted-foreground">E-mail</dt><dd>{{ basic?.email }}</dd></div>
                <div><dt class="text-muted-foreground">手機</dt><dd>{{ basic?.mobilePhone }}</dd></div>
                <div class="sm:col-span-2"><dt class="text-muted-foreground">戶籍地址</dt><dd>{{ basic?.homeCity }}{{ basic?.homeDistrict }}{{ basic?.homeAddress }}</dd></div>
              </dl>
            </section>

            <section class="mb-6">
              <h3 class="mb-2 font-medium text-primary">工作經歷</h3>
              <div
                v-for="(w, i) in (work?.workExperiences as Record<string, unknown>[] || [])"
                :key="i"
                class="mb-3 rounded border p-3 text-sm"
              >
                <p class="font-medium">{{ w.companyName }} — {{ w.jobTitle }}</p>
                <p class="text-muted-foreground">{{ w.industryType }} · {{ w.startYear }}/{{ w.startMonth }} ~ {{ w.endYear }}/{{ w.endMonth }}</p>
              </div>
              <p v-if="!(work?.workExperiences as unknown[])?.length" class="text-sm text-muted-foreground">（無）</p>
            </section>

            <section>
              <h3 class="mb-2 font-medium text-primary">自我簡介</h3>
              <dl class="grid gap-2 text-sm">
                <div><dt class="text-muted-foreground">未來計劃</dt><dd class="whitespace-pre-wrap">{{ intro?.futurePlan }}</dd></div>
                <div><dt class="text-muted-foreground">應徵理由</dt><dd class="whitespace-pre-wrap">{{ intro?.applyReason }}</dd></div>
                <div><dt class="text-muted-foreground">希望待遇</dt><dd>{{ salaryLabel() }}</dd></div>
                <div><dt class="text-muted-foreground">最早報到</dt><dd>{{ intro?.earliestStartDate || '-' }}</dd></div>
              </dl>
            </section>
          </template>
        </div>
      </div>
    </div>
  </Teleport>
</template>
