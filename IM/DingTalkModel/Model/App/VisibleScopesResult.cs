using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.App
{
    public class VisibleScopesResult : BaseResult
    {
        [SimpleElement(NamingRule = NamingRule.Camel)]
        public bool IsHidden { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel, IsMultiple = true)]
        public List<int> DeptVisibleScopes { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel, IsMultiple = true)]
        public List<string> UserVisibleScopes { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public long AgentId { get; set; }
    }
}