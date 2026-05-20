export type FormDraft = {
  basic?: Record<string, unknown>
  work?: Record<string, unknown>
  intro?: Record<string, unknown>
  updatedAt?: string
}

const PREFIX = 'hr_draft_'

function key(userId: string) {
  return `${PREFIX}${userId}`
}

export function loadDraft(userId: string): FormDraft | null {
  if (!userId) return null
  try {
    const raw = localStorage.getItem(key(userId))
    return raw ? (JSON.parse(raw) as FormDraft) : null
  } catch {
    return null
  }
}

export function saveDraft(userId: string, draft: FormDraft) {
  if (!userId) return
  draft.updatedAt = new Date().toISOString()
  localStorage.setItem(key(userId), JSON.stringify(draft))
}

export function clearDraft(userId: string) {
  if (!userId) return
  localStorage.removeItem(key(userId))
}

export function hasDraft(userId: string) {
  return !!loadDraft(userId)
}
