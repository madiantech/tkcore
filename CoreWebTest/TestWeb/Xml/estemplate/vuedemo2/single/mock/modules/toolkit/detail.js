import Mock from 'mockjs'

const data = {
  "UR_PART": [{
    "Id": "1",
    "Name": "系统管理员",
    "Description": "管理系统，默认创建"
  }],
  "DetailOperator": [
    {
      "Id": "Update",
      "Caption": "修改",
      "Info": "Update",
      "Content": "/update/razortest_user.c?Id=*Id*",
      "ConfirmData": "",
      "IconClass": "icon-edit",
      "DialogTitle": "修改"
    }
  ],
  "URL": [
    {
      "RetURL": "http%3a%2f%2flocalhost%3a24885%2flist%2frazortest_user.c%3fPage%3d0%26Condition%3d%26Tab%3d%26TotalCount%3d8%26TotalPage%3d0%26Sort%3d-1%26Order%3dASC",
      "SelfURL": "http%3a%2f%2flocalhost%3a24885%2fdetail%2frazortest_user.c%3fId%3d1",
      "DRetURL": "http://localhost:24885/list/razortest_user.c?Page=0&Condition=&Tab=&TotalCount=8&TotalPage=0&Sort=-1&Order=ASC",
      "DSelfURL": "http://localhost:24885/detail/razortest_user.c?Id=1"
    }
  ],
  "Info": [
    {
      "UserId": "1",
      "RoleId": "1",
      "Source": "razortest_user",
      "Module": "true",
      "IsHttpPost": "false",
      "Guid": "92d29fd8-89b2-4a3f-b56c-203f870d927e",
      "PageX": "c",
      "SessionId": "pheaidqgto3z340qlolzjshe",
      "Culture": "zh-CHS",
      "Style": "Detail"
    }
  ]
}

export default [
  {
    url: '/c/xml/detail/razortest/partdata',
    type: 'get',
    response: config => {
      return data
      // const items = data.items
      // return {
      //   code: 20000,
      //   data: {
      //     total: items.length,
      //     items: items
      //   }
      // }
    }
  }
]
