<script setup lang="ts">
import { cn } from '@/lib/utils'

const model = defineModel<string>()
const props = defineProps<{ invalid?: boolean; fieldKey?: string }>()
</script>

<template>
  <textarea
    v-model="model"
    :data-field="fieldKey"
    :class="cn(
      'flex min-h-[80px] w-full rounded-md border bg-white px-3 py-2 text-sm focus-visible:outline-none focus-visible:ring-2',
      invalid ? 'border-destructive ring-2 ring-destructive/40 focus-visible:ring-destructive' : 'border-border focus-visible:ring-primary',
      $attrs.class as string,
    )"
  />
</template>
