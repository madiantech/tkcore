import Mock from 'mockjs'

const data = {
  "UR_PART": [{
    "Id": "1",
    "Name": "系统管理员",
    "Description": "管理系统，默认创建"
  }],
  "UR_SUB_PART": [{
    "Id": "1",
    "PartId": "1",
    "Name": "test",
    "Description": "dddddd"
  }, {
    "Id": "2",
    "PartId": "1",
    "Name": "普通",
    "Description": "管理系统，默认创建"
  }],
  "URL": [{
    "RetURL": "%2fc%2fxml%2flist%2fRazorTest%2fMDPart%3fPage%3d0%26Condition%3d%26Tab%3d%26TotalCount%3d151%26TotalPage%3d10%26JsonOrder%3d%257B%2522FieldList%2522%253A%255B%255D%257D",
    "SelfURL": "%2fc%2fxml%2fupdate%2fRazorTest%2fMDPart%3fId%3d1%26RetUrl%3dhttp%25253A%25252F%25252Flocalhost%25253A5000%25252Fc%25252Fxml%25252Flist%25252FRazorTest%25252FMDPart%25253FPage%25253D0%252526Condition%25253D%252526Tab%25253D%252526TotalCount%25253D151%252526TotalPage%25253D10%252526JsonOrder%25253D%2525257B%25252522FieldList%25252522%2525253A%2525255B%2525255D%2525257D",
    "DRetURL": "/c/xml/list/RazorTest/MDPart?Page=0&Condition=&Tab=&TotalCount=151&TotalPage=10&JsonOrder=%7B%22FieldList%22%3A%5B%5D%7D",
    "DSelfURL": "/c/xml/update/RazorTest/MDPart?Id=1&RetUrl=http%253A%252F%252Flocalhost%253A5000%252Fc%252Fxml%252Flist%252FRazorTest%252FMDPart%253FPage%253D0%2526Condition%253D%2526Tab%253D%2526TotalCount%253D151%2526TotalPage%253D10%2526JsonOrder%253D%25257B%252522FieldList%252522%25253A%25255B%25255D%25257D"
  }],
  "Info": [{
    "UserId": "1",
    "RoleId": "1",
    "Source": "RazorTest/MDPart",
    "Module": "true",
    "IsHttpPost": "false",
    "Guid": "00c3ed59-699f-4838-983d-cd20b068e4b4",
    "SessionId": "f4db5b9d-a280-19fc-1e4f-00c6b2a187d6",
    "Culture": "zh-CN",
    "Style": "Update"
  }],
  "QueryString": [{
    "Id": "1",
    "RetUrl": "http://localhost:5000/c/xml/list/RazorTest/MDPart?Page=0&Condition=&Tab=&TotalCount=151&TotalPage=10&JsonOrder=%7B%22FieldList%22%3A%5B%5D%7D"
  }]
}

const responseerrors = {
  "Result": {
    "Message": "校验错误",
    "Result": "Error"
  },
  "FieldInfo": [{
    "TableName": "UR_PART",
    "NickName": "Name",
    "Message": "籍贯不存在",
    "Position": "0"
  },
    {
      "TableName": "UR_SUB_PART",
      "NickName": "Name",
      "Message": "籍贯不存在",
      "Position": "1"
    }
  ]
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
    url: '/c/xml/update/razortest/multiedit',
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
    url: '/c/xml/update/razortest/multiedit',
    type: 'post',
    response: config => {
      return responseerrors
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
