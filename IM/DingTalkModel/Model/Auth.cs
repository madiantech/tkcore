using System.Collections.Generic;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    public class Auth : BaseResult
    {
        [SimpleElement(NamingRule = NamingRule.UnderLineLower, IsMultiple = true)]
        public List<string> AuthUserField { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower, IsMultiple = true)]
        public List<string> ConditionField { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower, IsMultiple = true)]
        public Authed AuthOrgScopes { get; set; }
    }
}