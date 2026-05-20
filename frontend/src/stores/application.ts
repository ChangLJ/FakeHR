import { defineStore } from 'pinia'
import { nextTick, reactive, ref, watch } from 'vue'
import { useDebounceFn } from '@vueuse/core'
import api from '@/api/client'
import { useAuthStore } from '@/stores/auth'
import { clearDraft, hasDraft, loadDraft, saveDraft } from '@/utils/draftStorage'
import { validateApplication } from '@/utils/formValidation'
import {
  createEmptyBasicForm,
  createEmptyIntroForm,
  createEmptyWorkForm,
  emptyEducation,
  emptyFamily,
  emptyLanguage,
  emptyCert,
  emptyRef,
  emptyWork,
  inferSalaryMode,
  type SalaryMode,
} from '@/stores/applicationFormDefaults'

export const useApplicationStore = defineStore('application', () => {
  const auth = useAuthStore()

  const data = ref<Record<string, unknown> | null>(null)
  const loading = ref(false)
  const saving = ref(false)
  const message = ref('')
  const error = ref('')
  const validationError = ref('')
  const draftRestored = ref(false)
  const invalidFields = ref<Set<string>>(new Set())

  const basicForm = reactive(createEmptyBasicForm())
  const workForm = reactive(createEmptyWorkForm())
  const introForm = reactive(createEmptyIntroForm())

  function isFieldInvalid(fieldKey: string) {
    return invalidFields.value.has(fieldKey)
  }

  function clearInvalidFields() {
    invalidFields.value = new Set()
  }

  function setInvalidFields(fields: string[]) {
    invalidFields.value = new Set(fields)
  }

  function scrollToInvalidField(field: string) {
    nextTick(() => {
      const el = document.querySelector(`[data-field="${field}"]`)
      el?.scrollIntoView({ behavior: 'smooth', block: 'center' })
    })
  }

  function ensureBasicLists() {
    if (!basicForm.educations?.length) basicForm.educations = [emptyEducation()]
    if (!basicForm.languages?.length) basicForm.languages = [emptyLanguage()]
    if (!basicForm.familyMembers?.length) basicForm.familyMembers = [emptyFamily()]
  }

  function ensureWorkLists() {
    if (!workForm.workExperiences?.length) workForm.workExperiences = [emptyWork()]
    while (workForm.references.length < 2) workForm.references.push(emptyRef())
  }

  function syncSalaryModeFromApi(selfIntro: Record<string, unknown>) {
    introForm.salaryMode = inferSalaryMode(selfIntro)
  }

  function applySalaryModeToPayload(selfIntro: Record<string, unknown>) {
    const mode = introForm.salaryMode as SalaryMode
    selfIntro.salaryByCompanyRule = mode === 'company'
    selfIntro.salaryNegotiable = mode === 'negotiable'
    if (mode === 'company' || mode === 'negotiable') {
      selfIntro.expectedMonthlySalary = ''
      selfIntro.expectedYearlySalary = ''
    }
  }

  function setSalaryMode(mode: SalaryMode) {
    introForm.salaryMode = mode
    introForm.salaryByCompanyRule = mode === 'company'
    introForm.salaryNegotiable = mode === 'negotiable'
    if (mode === 'company' || mode === 'negotiable') {
      introForm.expectedMonthlySalary = ''
      introForm.expectedYearlySalary = ''
    }
  }

  function hydrateFromApi(res: Record<string, unknown>) {
    const b = res.basic as Record<string, unknown> | undefined
    const w = res.work as Record<string, unknown> | undefined
    const s = res.selfIntro as Record<string, unknown> | undefined

    if (b) Object.assign(basicForm, b)
    if (w) Object.assign(workForm, w)
    if (s) {
      Object.assign(introForm, s)
      syncSalaryModeFromApi(s)
    }

    ensureBasicLists()
    ensureWorkLists()
  }

  function applyDraft() {
    const draft = loadDraft(auth.userId)
    if (!draft) return false
    if (draft.basic) Object.assign(basicForm, draft.basic)
    if (draft.work) Object.assign(workForm, draft.work)
    if (draft.intro) {
      Object.assign(introForm, draft.intro)
      if (draft.intro.salaryMode) {
        introForm.salaryMode = draft.intro.salaryMode as SalaryMode
      } else {
        syncSalaryModeFromApi(draft.intro)
      }
    }
    ensureBasicLists()
    ensureWorkLists()
    draftRestored.value = true
    return true
  }

  const persistDraft = useDebounceFn(() => {
    if (!auth.userId) return
    saveDraft(auth.userId, {
      basic: { ...basicForm },
      work: { ...workForm },
      intro: { ...introForm },
    })
  }, 400)

  watch([basicForm, workForm, introForm], persistDraft, { deep: true })

  watch(
    () => introForm.salaryMode,
    (mode) => {
      if (!mode) return
      introForm.salaryByCompanyRule = mode === 'company'
      introForm.salaryNegotiable = mode === 'negotiable'
      if (mode === 'company' || mode === 'negotiable') {
        introForm.expectedMonthlySalary = ''
        introForm.expectedYearlySalary = ''
      }
    },
  )

  async function fetchApplication() {
    loading.value = true
    message.value = ''
    error.value = ''
    validationError.value = ''
    draftRestored.value = false
    clearInvalidFields()
    try {
      const { data: res } = await api.get('/application')
      data.value = res
      hydrateFromApi(res as Record<string, unknown>)
      const draft = loadDraft(auth.userId)
      const serverEmail = (res as { basic?: { email?: string } }).basic?.email
      const draftEmail = (draft?.basic as { email?: string } | undefined)?.email
      if (
        draft &&
        serverEmail?.includes('@demo.example.com') &&
        draftEmail &&
        !draftEmail.includes('@demo.example.com')
      ) {
        clearDraft(auth.userId)
      } else if (draft) {
        applyDraft()
      }
    } finally {
      loading.value = false
    }
  }

  function buildSavePayload() {
    const selfIntro = { ...introForm } as Record<string, unknown>
    applySalaryModeToPayload(selfIntro)
    return {
      basic: { ...basicForm },
      work: { ...workForm },
      selfIntro,
    }
  }

  async function saveApplication() {
    validationError.value = ''
    message.value = ''
    error.value = ''
    clearInvalidFields()

    const result = validateApplication(
      basicForm as unknown as Record<string, unknown>,
      workForm as unknown as Record<string, unknown>,
      introForm as unknown as Record<string, unknown>,
    )
    if (!result.ok) {
      validationError.value = result.message
      setInvalidFields(result.fields)
      scrollToInvalidField(result.field)
      return false
    }

    saving.value = true
    try {
      const { data: res } = await api.put('/application', buildSavePayload())
      data.value = res
      hydrateFromApi(res as Record<string, unknown>)
      clearDraft(auth.userId)
      draftRestored.value = false
      message.value = '申請表已儲存'
      return true
    } catch {
      error.value = '儲存失敗，請稍後再試'
      return false
    } finally {
      saving.value = false
    }
  }

  return {
    data,
    loading,
    saving,
    message,
    error,
    validationError,
    draftRestored,
    basicForm,
    workForm,
    introForm,
    isFieldInvalid,
    setSalaryMode,
    fetchApplication,
    saveApplication,
    emptyEducation,
    emptyLanguage,
    emptyFamily,
    emptyWork,
    emptyCert,
    emptyRef,
  }
})
