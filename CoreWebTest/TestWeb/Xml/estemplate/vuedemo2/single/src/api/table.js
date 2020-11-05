import request from '@/tkcore/utils/request'

export function getList(params) {
  return request({
    url: '/vue-admin-template/toolkit/table/list',
    method: 'get',
    params
  })
}
