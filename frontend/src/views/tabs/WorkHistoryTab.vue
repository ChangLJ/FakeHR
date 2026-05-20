<script setup lang="ts">
import { Plus, Trash2 } from 'lucide-vue-next'
import { useApplicationStore } from '@/stores/application'
import Card from '@/components/ui/Card.vue'
import Button from '@/components/ui/Button.vue'
import Input from '@/components/ui/Input.vue'
import SelectField from '@/components/ui/SelectField.vue'
import FormSection from '@/components/FormSection.vue'
import FormRow from '@/components/FormRow.vue'
import FormField from '@/components/FormField.vue'
import { ynOptions, monthOptions } from '@/constants/options'

const appStore = useApplicationStore()
const form = appStore.workForm
const inv = appStore.isFieldInvalid
</script>

<template>
  <Card class="p-6">
    <FormSection title="最近工作經歷">
      <div v-for="(work, i) in form.workExperiences" :key="i" class="mb-6 rounded-lg border border-cyan-100 bg-cyan-50/30 p-4">
        <div class="mb-2 flex items-center justify-between">
          <span class="font-medium text-primary">#{{ i + 1 }}</span>
          <Button v-if="form.workExperiences.length > 1" variant="ghost" size="sm" class="text-destructive" @click="form.workExperiences.splice(i, 1)">
            <Trash2 class="h-4 w-4" /> 刪除
          </Button>
        </div>
        <FormRow>
          <FormField label="公司名稱"><Input v-model="work.companyName as string" /></FormField>
          <FormField label="職稱"><Input v-model="work.jobTitle as string" /></FormField>
          <FormField label="行業別"><Input v-model="work.industryType as string" /></FormField>
          <FormField label="月薪"><Input v-model="work.monthlySalary as string" /></FormField>
          <FormField label="年薪"><Input v-model="work.yearlySalary as string" /></FormField>
          <FormField label="起始年"><Input :model-value="String(work.startYear ?? '')" type="number" @update:model-value="work.startYear = $event ? Number($event) : null" /></FormField>
          <FormField label="起始月"><SelectField v-model="work.startMonth as string" :options="monthOptions" /></FormField>
          <FormField label="結束年"><Input :model-value="String(work.endYear ?? '')" type="number" @update:model-value="work.endYear = $event ? Number($event) : null" /></FormField>
          <FormField label="結束月"><SelectField v-model="work.endMonth as string" :options="monthOptions" /></FormField>
        </FormRow>
        <FormRow class="mt-3">
          <FormField label="離職原因" class="md:col-span-2"><Input v-model="work.leaveReason as string" /></FormField>
          <FormField label="自願離職"><SelectField v-model="work.isVoluntaryLeave as string" :options="ynOptions" /></FormField>
          <FormField label="主管姓名"><Input v-model="work.supervisorName as string" /></FormField>
          <FormField label="主管職稱"><Input v-model="work.supervisorTitle as string" /></FormField>
          <FormField label="工作內容" class="md:col-span-3"><Input v-model="work.workDescription as string" /></FormField>
        </FormRow>
      </div>
      <Button variant="outline" size="sm" @click="form.workExperiences.push(appStore.emptyWork())"><Plus class="mr-1 h-4 w-4" />新增工作經歷</Button>
    </FormSection>

    <FormSection title="證照">
      <div v-for="(cert, i) in form.certificates" :key="i" class="mb-3 rounded border border-border p-3">
        <FormRow>
          <FormField label="證照名稱"><Input v-model="cert.name as string" /></FormField>
          <FormField label="認證字號"><Input v-model="cert.licenseNumber as string" /></FormField>
          <FormField label="等級"><Input v-model="cert.level as string" /></FormField>
          <FormField label="分數"><Input v-model="cert.score as string" /></FormField>
        </FormRow>
        <Button variant="ghost" size="sm" class="text-destructive" @click="form.certificates.splice(i, 1)"><Trash2 class="h-4 w-4" /></Button>
      </div>
      <Button variant="outline" size="sm" @click="form.certificates.push(appStore.emptyCert())"><Plus class="mr-1 h-4 w-4" />新增證照</Button>
    </FormSection>

    <FormSection title="聯絡人資訊 (兩位)">
      <div v-for="(ref, i) in form.references" :key="i" class="mb-4 rounded border border-border p-4">
        <p class="mb-2 font-medium">聯絡人 {{ i + 1 }}</p>
        <FormRow>
          <FormField label="姓名" :field-key="`work.references.${i}.name`"><Input v-model="ref.name as string" :field-key="`work.references.${i}.name`" :invalid="inv(`work.references.${i}.name`)" /></FormField>
          <FormField label="關係" :field-key="`work.references.${i}.relation`"><Input v-model="ref.relation as string" :field-key="`work.references.${i}.relation`" :invalid="inv(`work.references.${i}.relation`)" /></FormField>
          <FormField label="公司" :field-key="`work.references.${i}.company`"><Input v-model="ref.company as string" :field-key="`work.references.${i}.company`" :invalid="inv(`work.references.${i}.company`)" /></FormField>
          <FormField label="職稱"><Input v-model="ref.jobTitle as string" /></FormField>
          <FormField label="行動電話" :field-key="`work.references.${i}.mobilePhone`"><Input v-model="ref.mobilePhone as string" :field-key="`work.references.${i}.mobilePhone`" :invalid="inv(`work.references.${i}.mobilePhone`)" /></FormField>
        </FormRow>
      </div>
      <label class="flex items-center gap-2 text-sm">
        <input v-model="form.allowContactCurrentEmployer" type="radio" :value="true" name="ask" /> 同意聯絡目前在職單位
        <input v-model="form.allowContactCurrentEmployer" type="radio" :value="false" name="ask" class="ml-4" /> 不同意
      </label>
    </FormSection>
  </Card>
</template>
