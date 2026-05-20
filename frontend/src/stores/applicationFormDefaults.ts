export function emptyEducation() {
  return {
    degreeType: '3', schoolName: '', major: '', minor: '',
    isDayDivision: 'Y', isGraduated: 'Y',
    startYear: null as number | null, startMonth: '', endYear: null as number | null, endMonth: '',
  }
}

export function emptyLanguage() {
  return {
    languageClass: 'A', otherDescription: '',
    listeningLevel: '2', speakingLevel: '2', readingLevel: '2', writingLevel: '2',
  }
}

export function emptyFamily() {
  return { relation: '02', name: '', age: '', company: '' }
}

export function emptyWork() {
  return {
    companyName: '', jobTitle: '', industryType: '', monthlySalary: '', yearlySalary: '',
    startYear: null as number | null, startMonth: '', endYear: null as number | null, endMonth: '',
    leaveReason: '', isVoluntaryLeave: 'Y', supervisorName: '', supervisorTitle: '', workDescription: '',
  }
}

export function emptyCert() {
  return { name: '', licenseNumber: '', level: '', score: '' }
}

export function emptyRef() {
  return {
    name: '', relation: '', company: '', jobTitle: '',
    companyPhoneArea: '', companyPhoneNumber: '', companyPhoneExt: '', mobilePhone: '',
  }
}

export function createEmptyBasicForm() {
  return {
    recruitPosition: '', idNumber: '', formDate: '', chineseName: '', englishName: '',
    marriageStatus: '', gender: '', bloodType: '', birthYear: null as number | null,
    birthMonth: null as number | null, birthDay: null as number | null,
    birthPlace: '', email: '', mobilePhone: '',
    homePhoneArea: '', homePhoneNumber: '', homePhoneExt: '',
    contactPhoneArea: '', contactPhoneNumber: '', contactPhoneExt: '',
    disabilityRank: '0', aboriginalStatus: 'N',
    homeZipCode: '', homeCity: '', homeDistrict: '', homeAddress: '',
    mailingZipCode: '', mailingCity: '', mailingDistrict: '', mailingAddress: '',
    emergencyRelation: '', emergencyName: '',
    emergencyPhone1Area: '', emergencyPhone1Number: '', emergencyPhone1Ext: '',
    emergencyPhone2Area: '', emergencyPhone2Number: '', emergencyPhone2Ext: '',
    emergencyAddress: '', sourceChannel: '1', sourceMemo: '',
    armyType: '0', armyOtherReason: '', armyPeriodStart: '', armyPeriodEnd: '',
    armyClass: '', armyBranch: '', agreedToTerms: false,
    educations: [emptyEducation()] as Record<string, unknown>[],
    languages: [emptyLanguage()] as Record<string, unknown>[],
    familyMembers: [emptyFamily()] as Record<string, unknown>[],
  }
}

export function createEmptyWorkForm() {
  return {
    workExperiences: [emptyWork()] as Record<string, unknown>[],
    certificates: [] as Record<string, unknown>[],
    references: [emptyRef(), emptyRef()] as Record<string, unknown>[],
    allowContactCurrentEmployer: false,
  }
}

export type SalaryMode = '' | 'company' | 'negotiable' | 'custom'

export function createEmptyIntroForm() {
  return {
    interests: '',
    futurePlan: '',
    applyReason: '',
    autobiography: '',
    domesticTravelWilling: false,
    domesticTravelPlace: '',
    overseasTravelWilling: false,
    overseasTravelPlace: '',
    expectedMonthlySalary: '',
    expectedYearlySalary: '',
    salaryMode: '' as SalaryMode,
    salaryByCompanyRule: false,
    salaryNegotiable: false,
    earliestStartDate: '',
  }
}

export function inferSalaryMode(intro: Record<string, unknown>): SalaryMode {
  if (intro.salaryByCompanyRule) return 'company'
  if (intro.salaryNegotiable) return 'negotiable'
  if (intro.expectedMonthlySalary || intro.expectedYearlySalary) return 'custom'
  return ''
}
