<script setup lang="ts">
defineProps<{ title: string; required?: boolean }>()
</script>

<template>
  <div class="mb-6">
    <h3 class="mb-3 flex items-center gap-1 text-base font-semibold text-slate-800">
      {{ title }}
      <span v-if="required" class="text-destructive">※</span>
    </h3>
    <slot />
  </div>
</template>
