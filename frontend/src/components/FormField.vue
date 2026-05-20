<script setup lang="ts">
defineProps<{ label: string; required?: boolean; fieldKey?: string }>()
</script>

<template>
  <div class="space-y-1.5" :data-field="fieldKey">
    <label class="text-sm font-medium text-slate-700">
      {{ label }}<span v-if="required" class="ml-0.5 text-destructive">※</span>
    </label>
    <slot />
  </div>
</template>
