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
import {
  marriageOptions, genderOptions, bloodOptions, ynOptions,
  armyOptions, degreeOptions, langClassOptions, abilityOptions,
  relationOptions,
} from '@/constants/options'

const appStore = useApplicationStore()
const form = appStore.basicForm
const inv = appStore.isFieldInvalid

function copyAddress() {
  form.mailingZipCode = form.homeZipCode
  form.mailingCity = form.homeCity
  form.mailingDistrict = form.homeDistrict
  form.mailingAddress = form.homeAddress
}
</script>

<template>
  <Card class="p-6">
    <FormSection title="個人基本資料" required>
      <FormRow>
        <FormField label="應徵項目"><Input v-model="form.recruitPosition" /></FormField>
        <FormField label="身份證/護照號" required><Input v-model="form.idNumber" disabled /></FormField>
        <FormField label="填表日期"><Input v-model="form.formDate" type="date" disabled /></FormField>
        <FormField label="中文姓名" required field-key="basic.chineseName"><Input v-model="form.chineseName" field-key="basic.chineseName" :invalid="inv('basic.chineseName')" /></FormField>
        <FormField label="英文姓名" required field-key="basic.englishName"><Input v-model="form.englishName" field-key="basic.englishName" :invalid="inv('basic.englishName')" /></FormField>
        <FormField label="婚姻狀況" required field-key="basic.marriageStatus"><SelectField v-model="form.marriageStatus" field-key="basic.marriageStatus" :invalid="inv('basic.marriageStatus')" :options="marriageOptions" /></FormField>
        <FormField label="性別" required field-key="basic.gender"><SelectField v-model="form.gender" field-key="basic.gender" :invalid="inv('basic.gender')" :options="genderOptions" /></FormField>
        <FormField label="血型"><SelectField v-model="form.bloodType" :options="bloodOptions" /></FormField>
        <FormField label="出生年" required field-key="basic.birthYear"><Input field-key="basic.birthYear" :invalid="inv('basic.birthYear')" :model-value="String(form.birthYear ?? '')" type="number" @update:model-value="form.birthYear = $event ? Number($event) : null" /></FormField>
        <FormField label="月"><Input :model-value="String(form.birthMonth ?? '')" type="number" @update:model-value="form.birthMonth = $event ? Number($event) : null" /></FormField>
        <FormField label="日"><Input :model-value="String(form.birthDay ?? '')" type="number" @update:model-value="form.birthDay = $event ? Number($event) : null" /></FormField>
        <FormField label="出生地" required field-key="basic.birthPlace"><Input v-model="form.birthPlace" field-key="basic.birthPlace" :invalid="inv('basic.birthPlace')" /></FormField>
        <FormField label="E-mail" required field-key="basic.email"><Input v-model="form.email" type="email" field-key="basic.email" :invalid="inv('basic.email')" /></FormField>
        <FormField label="行動電話" required field-key="basic.mobilePhone"><Input v-model="form.mobilePhone" field-key="basic.mobilePhone" :invalid="inv('basic.mobilePhone')" /></FormField>
      </FormRow>
    </FormSection>

    <FormSection title="聯絡地址" required>
      <p class="mb-2 text-sm font-medium">戶籍地址</p>
      <FormRow>
        <FormField label="郵遞區號" field-key="basic.homeZipCode"><Input v-model="form.homeZipCode" field-key="basic.homeZipCode" :invalid="inv('basic.homeZipCode')" /></FormField>
        <FormField label="縣市" field-key="basic.homeCity"><Input v-model="form.homeCity" field-key="basic.homeCity" :invalid="inv('basic.homeCity')" /></FormField>
        <FormField label="鄉鎮市區" field-key="basic.homeDistrict"><Input v-model="form.homeDistrict" field-key="basic.homeDistrict" :invalid="inv('basic.homeDistrict')" /></FormField>
        <FormField label="地址" class="md:col-span-2" field-key="basic.homeAddress"><Input v-model="form.homeAddress" field-key="basic.homeAddress" :invalid="inv('basic.homeAddress')" /></FormField>
      </FormRow>
      <p class="mb-2 mt-4 text-sm font-medium">
        通訊地址
        <button type="button" class="ml-2 text-primary underline" @click="copyAddress">同上</button>
      </p>
      <FormRow>
        <FormField label="郵遞區號" field-key="basic.mailingZipCode"><Input v-model="form.mailingZipCode" field-key="basic.mailingZipCode" :invalid="inv('basic.mailingZipCode')" /></FormField>
        <FormField label="縣市" field-key="basic.mailingCity"><Input v-model="form.mailingCity" field-key="basic.mailingCity" :invalid="inv('basic.mailingCity')" /></FormField>
        <FormField label="鄉鎮市區" field-key="basic.mailingDistrict"><Input v-model="form.mailingDistrict" field-key="basic.mailingDistrict" :invalid="inv('basic.mailingDistrict')" /></FormField>
        <FormField label="地址" field-key="basic.mailingAddress"><Input v-model="form.mailingAddress" field-key="basic.mailingAddress" :invalid="inv('basic.mailingAddress')" /></FormField>
      </FormRow>
    </FormSection>

    <FormSection title="教育程度" required>
      <div v-for="(edu, i) in form.educations" :key="i" class="mb-4 rounded border border-border p-4">
        <FormRow>
          <FormField label="學位"><SelectField v-model="edu.degreeType as string" :options="degreeOptions" /></FormField>
          <FormField label="學校" :field-key="`basic.educations.${i}.schoolName`"><Input v-model="edu.schoolName as string" :field-key="`basic.educations.${i}.schoolName`" :invalid="inv(`basic.educations.${i}.schoolName`)" /></FormField>
          <FormField label="主修"><Input v-model="edu.major as string" /></FormField>
          <FormField label="日間部"><SelectField v-model="edu.isDayDivision as string" :options="ynOptions" /></FormField>
          <FormField label="畢業"><SelectField v-model="edu.isGraduated as string" :options="ynOptions" /></FormField>
        </FormRow>
        <Button v-if="form.educations.length > 1" variant="ghost" size="sm" class="mt-2 text-destructive" @click="form.educations.splice(i, 1)">
          <Trash2 class="h-4 w-4" /> 刪除
        </Button>
      </div>
      <Button variant="outline" size="sm" @click="form.educations.push(appStore.emptyEducation())"><Plus class="mr-1 h-4 w-4" />新增教育程度</Button>
    </FormSection>

    <FormSection title="外語能力" required>
      <div v-for="(lang, i) in form.languages" :key="i" class="mb-4 rounded border border-border p-4">
        <FormRow>
          <FormField label="語言"><SelectField v-model="lang.languageClass as string" :options="langClassOptions" /></FormField>
          <FormField label="聽"><SelectField v-model="lang.listeningLevel as string" :options="abilityOptions" /></FormField>
          <FormField label="說"><SelectField v-model="lang.speakingLevel as string" :options="abilityOptions" /></FormField>
          <FormField label="讀"><SelectField v-model="lang.readingLevel as string" :options="abilityOptions" /></FormField>
          <FormField label="寫"><SelectField v-model="lang.writingLevel as string" :options="abilityOptions" /></FormField>
        </FormRow>
        <Button v-if="form.languages.length > 1" variant="ghost" size="sm" class="mt-2 text-destructive" @click="form.languages.splice(i, 1)"><Trash2 class="h-4 w-4" /> 刪除</Button>
      </div>
      <Button variant="outline" size="sm" @click="form.languages.push(appStore.emptyLanguage())"><Plus class="mr-1 h-4 w-4" />新增外語能力</Button>
    </FormSection>

    <FormSection title="家庭成員" required>
      <div v-for="(fm, i) in form.familyMembers" :key="i" class="mb-4 rounded border border-border p-4">
        <FormRow>
          <FormField label="稱謂"><SelectField v-model="fm.relation as string" :options="relationOptions" /></FormField>
          <FormField label="姓名" :field-key="`basic.familyMembers.${i}.name`"><Input v-model="fm.name as string" :field-key="`basic.familyMembers.${i}.name`" :invalid="inv(`basic.familyMembers.${i}.name`)" /></FormField>
          <FormField label="年齡"><Input v-model="fm.age as string" /></FormField>
          <FormField label="服務單位"><Input v-model="fm.company as string" /></FormField>
        </FormRow>
        <Button v-if="form.familyMembers.length > 1" variant="ghost" size="sm" class="mt-2 text-destructive" @click="form.familyMembers.splice(i, 1)"><Trash2 class="h-4 w-4" /> 刪除</Button>
      </div>
      <Button variant="outline" size="sm" @click="form.familyMembers.push(appStore.emptyFamily())"><Plus class="mr-1 h-4 w-4" />新增家庭成員</Button>
    </FormSection>

    <FormSection title="兵役狀況 (女性免填)">
      <FormRow>
        <FormField label="兵役狀況"><SelectField v-model="form.armyType" :options="armyOptions" /></FormField>
        <FormField label="原因"><Input v-model="form.armyOtherReason" :disabled="!['4','5'].includes(form.armyType)" /></FormField>
        <FormField label="服役起"><Input v-model="form.armyPeriodStart" type="date" /></FormField>
        <FormField label="服役迄"><Input v-model="form.armyPeriodEnd" type="date" /></FormField>
      </FormRow>
    </FormSection>

    <label
      data-field="basic.agreedToTerms"
      class="flex items-center gap-2 rounded-md border p-3 text-sm font-medium"
      :class="inv('basic.agreedToTerms') ? 'border-destructive ring-2 ring-destructive/40' : 'border-transparent'"
    >
      <input v-model="form.agreedToTerms" type="checkbox" class="rounded" />
      以上內容經本人詳細閱讀並據實填寫。若有不實或欺瞞之處，願接受無條件解僱。
    </label>
  </Card>
</template>
