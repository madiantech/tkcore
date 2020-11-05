const files = require.context('.', true, /\index.js$/)
let directives = {}
files.keys().forEach(key => {
  if (key === './index.js') return
  const directive = files(key).default
  let dName = key.split('/')[1]
  const name = 'tk' + dName.charAt(0).toUpperCase() + dName.slice(1)// 转换为 tkDialog 形式的名称
  directives[name] = directive
})
export default directives