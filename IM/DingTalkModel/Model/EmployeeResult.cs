using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    public class EmployeeResult : BaseResult
    {
        [ObjectElement(NamingRule = NamingRule.Camel, IsMultiple = true)]
        public List<EmployeeInfo> Result { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public bool Success { get; set; }
    }
}