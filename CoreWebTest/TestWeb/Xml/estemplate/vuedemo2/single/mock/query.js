import Mock from 'mockjs'

const data = Mock.mock({
  'items|6': [{
    id: '@id',
    title: '@sentence(1,2)'
  }]
})

export default [
  {
    url: '/vue-admin-template/toolkit/query',
    type: 'get',
    response: config => {
      const items = data.items
      return {
        code: 20000,
        data: {
          total: items.length,
          items: items
        }
      }
    }
  }
]
