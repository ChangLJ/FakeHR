<script setup lang="ts">
import { cn } from '@/lib/utils'

defineOptions({ inheritAttrs: false })
const model = defineModel<string>()
const props = defineProps<{ invalid?: boolean; fieldKey?: string }>()
</script>

<template>
  <input
    v-model="model"
    v-bind="$attrs"
    :data-field="fieldKey"
    :class="cn(
      'flex h-9 w-full rounded-md border bg-white px-3 py-1 text-sm shadow-sm placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-2 disabled:cursor-not-allowed disabled:opacity-50',
      invalid ? 'border-destructive ring-2 ring-destructive/40 focus-visible:ring-destructive' : 'border-border focus-visible:ring-primary',
      $attrs.class as string,
    )"
  />
</template>
