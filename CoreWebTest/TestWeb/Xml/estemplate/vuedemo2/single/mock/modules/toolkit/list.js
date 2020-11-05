import Mock from 'mockjs'

const data = {
  "ListOperator": [{
    "Id": "Insert",
    "Caption": "新建",
    "Info": "Insert",
    "Content": "/c/xml/insert/razortest/partdata",
    "ConfirmData": "",
    "IconClass": "icon-plus",
    "DialogTitle": "新建"
  }],
  "Count": [{
    "TotalCount": "4",
    "TotalPage": "0",
    "CurrentPage": "0",
    "PageSize": "15"
  }],
  "Sort": [{
    "SqlCon": "",
    "JsonOrder": "{\"FieldList\": [{\"NickName\": \"Name\",\"Order\": \"ASC\"}]}"
  }],
  "UR_PART": [{
    "Id": "1",
    "Name": "<a data-url='/c/xml/detail/razortest/partdata?Id=1' href='#'>系统管理员</a>",
    "Description": "管理系统，默认创建",
    "ROWNUMBER_": "1",
    "_OPERATOR_RIGHT": "|Update|Delete|"
  }, {
    "Id": "1001",
    "Name": "<a data-url='/c/xml/detail/razortest/partdata?Id=1001' href='#'>test</a>",
    "Description": "",
    "ROWNUMBER_": "2",
    "_OPERATOR_RIGHT": "|Update|Delete|"
  }, {
    "Id": "1002",
    "Name": "<a data-url='/c/xml/detail/razortest/partdata?Id=1002' href='#'>test3</a>",
    "Description": "test3",
    "ROWNUMBER_": "3",
    "_OPERATOR_RIGHT": "|Update|Delete|"
  }, {
    "Id": "1003",
    "Name": "<a data-url='/c/xml/detail/razortest/partdata?Id=1003' href='#'>223</a>",
    "Description": "2323",
    "ROWNUMBER_": "4",
    "_OPERATOR_RIGHT": "|Update|Delete|"
  }],
  "RowOperator": [{
    "Id": "Update",
    "Caption": "修改",
    "Info": "Update",
    "Content": "/c/xml/update/razortest/partdata?Id=*Id*",
    "ConfirmData": "",
    "IconClass": "icon-edit",
    "DialogTitle": "修改"
  }, {
    "Id": "Delete",
    "Caption": "删除",
    "Info": "Delete,AjaxUrl",
    "Content": "/c/xml/delete/razortest/partdata?Id=*Id*",
    "ConfirmData": "确认删除吗？",
    "IconClass": "icon-remove",
    "DialogTitle": "删除"
  }],
  "URL": [{
    "RetURL": "",
    "SelfURL": "http%3a%2f%2flocalhost%3a5000%2fc%2fxml%2flist%2frazortest%2fpartdata",
    "DRetURL": "",
    "DSelfURL": "http://localhost:5000/c/xml/list/razortest/partdata"
  }],
  "Info": [{
    "UserId": "1",
    "RoleId": "1017",
    "Source": "razortest/partdata",
    "Module": "true",
    "IsHttpPost": "false",
    "Guid": "6df0ae42-c775-403e-8f61-18b6bb4edb0a",
    "SessionId": "97c9d67a-0779-df1c-31a7-58463ccec874",
    "Culture": "zh-CN",
    "Style": "List"
  }],
  "QueryString": [{}]
}

export default [
  {
    url: '/c/xml/list/RazorTest/Part',
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
    url: '/c/xml/list/RazorTest/Part',
    type: 'post',
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
