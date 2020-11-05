using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    internal class EmployeeParam
    {
        public EmployeeParam()
        {
        }

        public EmployeeParam(string userIdList)
        {
            UserIdList = userIdList;
        }

        public EmployeeParam(string userIdList, string fieldFilterList)
        {
            UserIdList = userIdList;
            FieldFilterList = fieldFilterList;
        }

        [SimpleElement(LocalName = "userid_list")]
        public string UserIdList { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string FieldFilterList { get; set; }
    }
}