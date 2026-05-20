<script setup lang="ts">
import { cn } from '@/lib/utils'
import { computed } from 'vue'

const props = withDefaults(defineProps<{
  variant?: 'default' | 'outline' | 'ghost' | 'destructive'
  size?: 'default' | 'sm' | 'lg'
  type?: 'button' | 'submit'
  disabled?: boolean
}>(), {
  variant: 'default',
  size: 'default',
  type: 'button',
})

const classes = computed(() => {
  const base = 'inline-flex items-center justify-center rounded-md text-sm font-medium transition-colors focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-primary disabled:opacity-50'
  const variants: Record<string, string> = {
    default: 'bg-primary text-primary-foreground hover:opacity-90',
    outline: 'border border-border bg-white hover:bg-muted',
    ghost: 'hover:bg-muted',
    destructive: 'bg-destructive text-white hover:opacity-90',
  }
  const sizes: Record<string, string> = {
    default: 'h-9 px-4 py-2',
    sm: 'h-8 px-3',
    lg: 'h-10 px-6',
  }
  return cn(base, variants[props.variant], sizes[props.size])
})
</script>

<template>
  <button :type="type" :disabled="disabled" :class="cn(classes, $attrs.class as string)">
    <slot />
  </button>
</template>
