using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Company
{
    public class AuthResult : BaseResult
    {
        [SimpleElement(NamingRule = NamingRule.UnderLineLower, IsMultiple = true)]
        public List<string> AuthUserField { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower, IsMultiple = true)]
        public List<string> ConditionField { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public AuthScopesInfo AuthOrgScopes { get; set; }
    }
}