<script setup lang="ts">
import { cn } from '@/lib/utils'

const model = defineModel<string>({ required: true })
defineProps<{ tabs: { id: string; label: string }[] }>()
</script>

<template>
  <div class="flex flex-wrap gap-2 border-b border-border pb-2">
    <button
      v-for="tab in tabs"
      :key="tab.id"
      type="button"
      :class="cn(
        'rounded-md px-4 py-2 text-sm font-medium transition-colors',
        model === tab.id ? 'bg-primary text-primary-foreground' : 'bg-white text-muted-foreground hover:bg-muted',
      )"
      @click="model = tab.id"
    >
      {{ tab.label }}
    </button>
  </div>
</template>
