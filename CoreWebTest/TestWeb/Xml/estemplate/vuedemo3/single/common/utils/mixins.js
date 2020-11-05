import config from 'src/config'
let computed = {}, formData = {}, tableTemplete = {}
// 主表
const tableName = config.tableName
formData[tableName] = []
tableTemplete[tableName] = {}
computed[tableName] = function () {
  return this.formData[tableName] && this.formData[tableName].length > 0 ? this.formData[tableName][0] : {}
}
// 从表
const subTableNames = config.subTableNames || []
subTableNames.forEach(key => {
  formData[key] = []
  tableTemplete[key] = {}
  computed[key] = function () {
    return this.formData[key] || []
  }
})
export default {
  formData,
  tableTemplete,
  computed
}
export {
  formData,
  tableTemplete,
  computed
}