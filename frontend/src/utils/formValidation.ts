export type ValidationResult =
  | { ok: true }
  | { ok: false; message: string; field: string; fields: string[] }

function fail(message: string, field: string): ValidationResult {
  return { ok: false, message, field, fields: [field] }
}

function isBlank(v: unknown): boolean {
  return v == null || (typeof v === 'string' && !v.trim())
}

function isBlankNumber(v: unknown): boolean {
  return v == null || v === '' || Number.isNaN(Number(v))
}

export function validateBasic(form: Record<string, unknown>): ValidationResult {
  const checks: [string, string, string][] = [
    ['chineseName', 'basic.chineseName', '中文姓名'],
    ['englishName', 'basic.englishName', '英文姓名'],
    ['marriageStatus', 'basic.marriageStatus', '婚姻狀況'],
    ['gender', 'basic.gender', '性別'],
    ['birthPlace', 'basic.birthPlace', '出生地'],
    ['email', 'basic.email', 'E-mail'],
    ['mobilePhone', 'basic.mobilePhone', '行動電話'],
    ['homeZipCode', 'basic.homeZipCode', '戶籍郵遞區號'],
    ['homeCity', 'basic.homeCity', '戶籍縣市'],
    ['homeDistrict', 'basic.homeDistrict', '戶籍鄉鎮市區'],
    ['homeAddress', 'basic.homeAddress', '戶籍地址'],
    ['mailingZipCode', 'basic.mailingZipCode', '通訊郵遞區號'],
    ['mailingCity', 'basic.mailingCity', '通訊縣市'],
    ['mailingDistrict', 'basic.mailingDistrict', '通訊鄉鎮市區'],
    ['mailingAddress', 'basic.mailingAddress', '通訊地址'],
  ]
  for (const [key, field, label] of checks) {
    if (isBlank(form[key])) return fail(`請填寫${label}`, field)
  }
  if (isBlankNumber(form.birthYear)) return fail('請填寫出生年', 'basic.birthYear')

  const educations = (form.educations as Record<string, unknown>[]) || []
  if (!educations.length) return fail('請至少新增一筆教育程度', 'basic.educations.0.schoolName')
  for (let i = 0; i < educations.length; i++) {
    if (isBlank(educations[i].schoolName))
      return fail(`教育程度 #${i + 1}：請填寫學校`, `basic.educations.${i}.schoolName`)
  }

  const languages = (form.languages as Record<string, unknown>[]) || []
  if (!languages.length) return fail('請至少新增一筆外語能力', 'basic.languages.0.languageClass')

  const family = (form.familyMembers as Record<string, unknown>[]) || []
  if (!family.length) return fail('請至少新增一位家庭成員', 'basic.familyMembers.0.name')
  for (let i = 0; i < family.length; i++) {
    if (isBlank(family[i].name))
      return fail(`家庭成員 #${i + 1}：請填寫姓名`, `basic.familyMembers.${i}.name`)
  }

  if (!form.agreedToTerms) return fail('請勾選同意聲明', 'basic.agreedToTerms')

  const email = String(form.email).trim()
  if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email))
    return fail('E-mail 格式不正確', 'basic.email')

  return { ok: true }
}

export function validateWork(form: Record<string, unknown>): ValidationResult {
  const refs = (form.references as Record<string, unknown>[]) || []
  for (let i = 0; i < 2; i++) {
    const r = refs[i]
    if (!r) return fail(`請填寫聯絡人 ${i + 1}`, `work.references.${i}.name`)
    if (isBlank(r.name)) return fail(`聯絡人 ${i + 1}：請填寫姓名`, `work.references.${i}.name`)
    if (isBlank(r.relation)) return fail(`聯絡人 ${i + 1}：請填寫關係`, `work.references.${i}.relation`)
    if (isBlank(r.company)) return fail(`聯絡人 ${i + 1}：請填寫公司`, `work.references.${i}.company`)
    if (isBlank(r.mobilePhone)) return fail(`聯絡人 ${i + 1}：請填寫行動電話`, `work.references.${i}.mobilePhone`)
  }

  const works = (form.workExperiences as Record<string, unknown>[]) || []
  for (let i = 0; i < works.length; i++) {
    const w = works[i]
    const hasAny = !isBlank(w.companyName) || !isBlank(w.jobTitle)
    if (!hasAny) continue
    if (isBlank(w.companyName)) return fail(`工作經歷 #${i + 1}：請填寫公司名稱`, `work.workExperiences.${i}.companyName`)
    if (isBlank(w.jobTitle)) return fail(`工作經歷 #${i + 1}：請填寫職稱`, `work.workExperiences.${i}.jobTitle`)
  }

  return { ok: true }
}

export function validateSelfIntro(form: Record<string, unknown>): ValidationResult {
  if (isBlank(form.futurePlan)) return fail('請填寫個人未來3-5年計劃', 'intro.futurePlan')
  if (isBlank(form.applyReason)) return fail('請填寫選擇本公司或勝任理由', 'intro.applyReason')
  if (form.domesticTravelWilling && isBlank(form.domesticTravelPlace))
    return fail('請填寫國內出差/派駐地點', 'intro.domesticTravelPlace')
  if (form.overseasTravelWilling && isBlank(form.overseasTravelPlace))
    return fail('請填寫國外出差/派駐地點', 'intro.overseasTravelPlace')
  if (isBlank(form.earliestStartDate)) return fail('請填寫最早報到日', 'intro.earliestStartDate')

  const mode = form.salaryMode as string
  if (!mode) return fail('請選擇希望待遇（依公司規定或面議）', 'intro.salaryMode')
  if (mode === 'custom') {
    const hasAmount =
      !isBlank(form.expectedMonthlySalary) || !isBlank(form.expectedYearlySalary)
    if (!hasAmount) return fail('請填寫月薪或年薪', 'intro.expectedMonthlySalary')
  }

  return { ok: true }
}

export function validateApplication(
  basic: Record<string, unknown>,
  work: Record<string, unknown>,
  intro: Record<string, unknown>,
): ValidationResult {
  for (const fn of [() => validateBasic(basic), () => validateWork(work), () => validateSelfIntro(intro)]) {
    const r = fn()
    if (!r.ok) return r
  }
  return { ok: true }
}
