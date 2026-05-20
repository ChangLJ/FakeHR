<script setup lang="ts">
import { useApplicationStore } from '@/stores/application'
import type { SalaryMode } from '@/stores/applicationFormDefaults'
import Card from '@/components/ui/Card.vue'
import Input from '@/components/ui/Input.vue'
import Textarea from '@/components/ui/Textarea.vue'
import FormSection from '@/components/FormSection.vue'
import FormRow from '@/components/FormRow.vue'
import FormField from '@/components/FormField.vue'

const appStore = useApplicationStore()
const form = appStore.introForm
const inv = appStore.isFieldInvalid

function onSalaryMode(mode: SalaryMode) {
  appStore.setSalaryMode(mode)
}
</script>

<template>
  <Card class="p-6">
    <FormSection title="社交興趣、運動專長或嗜好 (最多3項,50字以內)">
      <Textarea v-model="form.interests" rows="2" maxlength="50" />
    </FormSection>

    <FormSection title="未來規劃與工作期許" required>
      <FormField label="個人未來3-5年計劃 (100字以內)" required field-key="intro.futurePlan">
        <Textarea v-model="form.futurePlan" rows="3" maxlength="100" field-key="intro.futurePlan" :invalid="inv('intro.futurePlan')" />
      </FormField>
      <FormField label="選擇本公司或勝任理由 (100字以內)" required class="mt-4" field-key="intro.applyReason">
        <Textarea v-model="form.applyReason" rows="3" maxlength="100" field-key="intro.applyReason" :invalid="inv('intro.applyReason')" />
      </FormField>
    </FormSection>

    <FormSection title="自傳">
      <Textarea v-model="form.autobiography" rows="12" class="min-h-[300px]" />
    </FormSection>

    <FormSection title="出差/派駐意願及工作地點" required>
      <div class="space-y-3">
        <label class="flex flex-wrap items-center gap-2 text-sm">
          <span class="w-32">國內出差/派駐</span>
          <input v-model="form.domesticTravelWilling" type="radio" :value="false" name="d1" /> 無意願
          <input v-model="form.domesticTravelWilling" type="radio" :value="true" name="d1" class="ml-2" /> 可，地點：
          <Input
            v-model="form.domesticTravelPlace"
            class="max-w-xs"
            field-key="intro.domesticTravelPlace"
            :invalid="inv('intro.domesticTravelPlace')"
            :disabled="!form.domesticTravelWilling"
          />
        </label>
        <label class="flex flex-wrap items-center gap-2 text-sm">
          <span class="w-32">國外出差/派駐</span>
          <input v-model="form.overseasTravelWilling" type="radio" :value="false" name="d2" /> 無意願
          <input v-model="form.overseasTravelWilling" type="radio" :value="true" name="d2" class="ml-2" /> 有，地點：
          <Input
            v-model="form.overseasTravelPlace"
            class="max-w-xs"
            field-key="intro.overseasTravelPlace"
            :invalid="inv('intro.overseasTravelPlace')"
            :disabled="!form.overseasTravelWilling"
          />
        </label>
      </div>
    </FormSection>

    <FormSection title="希望待遇與報到日期" required>
      <div
        data-field="intro.salaryMode"
        class="mb-4 flex flex-wrap gap-6 rounded-md border p-3 text-sm"
        :class="inv('intro.salaryMode') ? 'border-destructive ring-2 ring-destructive/40' : 'border-border'"
      >
        <span class="w-full font-medium text-slate-700">希望待遇 <span class="text-destructive">※</span></span>
        <label class="flex cursor-pointer items-center gap-2">
          <input type="radio" name="salaryMode" value="company" :checked="form.salaryMode === 'company'" @change="onSalaryMode('company')" />
          依公司規定
        </label>
        <label class="flex cursor-pointer items-center gap-2">
          <input type="radio" name="salaryMode" value="negotiable" :checked="form.salaryMode === 'negotiable'" @change="onSalaryMode('negotiable')" />
          面議
        </label>
        <label class="flex cursor-pointer items-center gap-2">
          <input type="radio" name="salaryMode" value="custom" :checked="form.salaryMode === 'custom'" @change="onSalaryMode('custom')" />
          自行填寫月薪/年薪
        </label>
      </div>
      <FormRow>
        <FormField label="月薪 (元)" field-key="intro.expectedMonthlySalary">
          <Input
            v-model="form.expectedMonthlySalary"
            field-key="intro.expectedMonthlySalary"
            :invalid="inv('intro.expectedMonthlySalary')"
            :disabled="form.salaryMode !== 'custom'"
          />
        </FormField>
        <FormField label="年薪 (萬元)">
          <Input
            v-model="form.expectedYearlySalary"
            :disabled="form.salaryMode !== 'custom'"
          />
        </FormField>
        <FormField label="最早報到日" field-key="intro.earliestStartDate">
          <Input
            v-model="form.earliestStartDate"
            type="date"
            field-key="intro.earliestStartDate"
            :invalid="inv('intro.earliestStartDate')"
          />
        </FormField>
      </FormRow>
    </FormSection>
  </Card>
</template>
