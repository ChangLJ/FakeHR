export const marriageOptions = [
  { value: '', label: '請選擇' },
  { value: '0', label: '未婚' },
  { value: '1', label: '已婚' },
]

export const genderOptions = [
  { value: '', label: '請選擇' },
  { value: '1', label: '男' },
  { value: '0', label: '女' },
]

export const bloodOptions = ['', 'A', 'B', 'AB', 'O'].map((v) => ({ value: v, label: v || '請選擇' }))

export const disabilityOptions = [
  { value: '0', label: '無' },
  { value: '1', label: '輕度' },
  { value: '2', label: '中度' },
  { value: '3', label: '重度' },
  { value: '5', label: '極重' },
]

export const ynOptions = [
  { value: '', label: '' },
  { value: 'N', label: 'N' },
  { value: 'Y', label: 'Y' },
]

export const sourceOptions = [
  { value: '1', label: '網路等電子媒體' },
  { value: '2', label: '報紙雜誌' },
  { value: '3', label: '校園徵才' },
  { value: '4', label: '親友介紹' },
  { value: '5', label: '關係企業轉調' },
  { value: '6', label: '人力仲介業者' },
  { value: '7', label: '其它(請說明)' },
]

export const armyOptions = [
  { value: '0', label: '無' },
  { value: '1', label: '義務役' },
  { value: '2', label: '志願役' },
  { value: '3', label: '國防/研發替代役' },
  { value: '4', label: '免役' },
  { value: '5', label: '其它' },
]

export const degreeOptions = [
  { value: '1', label: '高中職' },
  { value: '2', label: '專科' },
  { value: '3', label: '大學' },
  { value: '4', label: '碩士' },
  { value: '5', label: '博士' },
  { value: '6', label: '其它' },
]

export const langClassOptions = [
  { value: 'A', label: '英文' },
  { value: 'B', label: '日文' },
  { value: 'C', label: '其它' },
]

export const abilityOptions = [
  { value: '1', label: '不會' },
  { value: '2', label: '略通' },
  { value: '3', label: '中等' },
  { value: '4', label: '良好' },
  { value: '5', label: '精通' },
]

export const relationOptions = [
  { value: '01', label: '配偶' },
  { value: '02', label: '父母' },
  { value: '03', label: '子女' },
  { value: '04', label: '祖父母' },
  { value: '05', label: '兄弟姊妹' },
]

export const monthOptions = [
  { value: '', label: ' ' },
  ...Array.from({ length: 12 }, (_, i) => {
    const m = String(i + 1).padStart(2, '0')
    return { value: m, label: m }
  }),
]

export const yearOptions = (from = 1956, to = new Date().getFullYear()) => [
  { value: '', label: '' },
  ...Array.from({ length: to - from + 1 }, (_, i) => {
    const y = String(from + i)
    return { value: y, label: y }
  }).reverse(),
]
