using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.App
{
    internal class VisibleScopesParam
    {
        public VisibleScopesParam()
        {
        }

        public VisibleScopesParam(long agentId)
        {
            AgentId = agentId;
        }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public long AgentId { get; set; }
    }
}