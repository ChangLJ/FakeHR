<script setup lang="ts">
import { cn } from '@/lib/utils'

const model = defineModel<string>()
const props = defineProps<{ options: { value: string; label: string }[]; invalid?: boolean; fieldKey?: string }>()
</script>

<template>
  <select
    v-model="model"
    :data-field="fieldKey"
    :class="cn(
      'flex h-9 w-full rounded-md border bg-white px-3 text-sm focus-visible:outline-none focus-visible:ring-2',
      invalid ? 'border-destructive ring-2 ring-destructive/40 focus-visible:ring-destructive' : 'border-border focus-visible:ring-primary',
      $attrs.class as string,
    )"
  >
    <option v-for="opt in options" :key="opt.value" :value="opt.value">{{ opt.label }}</option>
  </select>
</template>
