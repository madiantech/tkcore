import Mock from 'mockjs'

const data = {
  "UR_PART": [{
    "Id": "1",
    "Name": "系统管理员",
    "Description": "管理系统，默认创建"
  }],
  "URL": [{
    "RetURL": "",
    "SelfURL": "%2fc%2fxml%2fupdate%2frazortest%2fpartdata%3fId%3d1",
    "DRetURL": "",
    "DSelfURL": "/c/xml/update/razortest/partdata?Id=1"
  }],
  "Info": [{
    "UserId": "1",
    "RoleId": "1",
    "Source": "razortest/partdata",
    "Module": "true",
    "IsHttpPost": "false",
    "Guid": "81b4f25c-dc53-4d86-9e09-0269d5a97b54",
    "SessionId": "1a530f91-f13d-2179-85b9-9ff11a60a785",
    "Culture": "zh-CN",
    "Style": "Update"
  }],
  "QueryString": [{
    "Id": "1"
  }]
}

const responseerrors = {
  "Result": {
    "Message": "校验错误",
    "Result": "Error"
  },
  "FieldInfo": [{
    "TableName": "UR_USERS",
    "NickName": "Name",
    "Message": "籍贯不存在",
    "Position": "0"
  }]
}

const response = {
  "NewWindow": false,
  "Result": {
    //"Message": "http://localhost:24885/usermanager_function.c?InitValue=6",
    "Message": "CloseDialogAndRefresh",
    "Result": "Success"
  }
}

export default [
  {
    url: '/c/xml/update/razortest/partdata',
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
  },
  {
    url: '/c/xml/update/razortest/partdata',
    type: 'post',
    response: config => {
      return response
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
