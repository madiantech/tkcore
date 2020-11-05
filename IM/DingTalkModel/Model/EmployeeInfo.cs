using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    public class EmployeeInfo
    {
        [SimpleElement(NamingRule = NamingRule.Lower)]
        public string UserId { get; set; }

        [ObjectElement(NamingRule = NamingRule.UnderLineLower, IsMultiple = true)]
        public List<FieldInfo> FieldList { get; set; }
    }
}