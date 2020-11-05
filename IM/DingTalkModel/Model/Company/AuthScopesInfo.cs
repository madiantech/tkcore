using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Company
{
    public class AuthScopesInfo
    {
        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public List<int> AuthedDept { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public List<string> AuthedUser { get; set; }
    }
}