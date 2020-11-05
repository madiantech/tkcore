using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    public class SpaceFileInfo
    {
        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string AgentId { get; set; }

        [SimpleElement(NamingRule = NamingRule.Lower)]
        public string UserId { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string MediaId { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string FileName { get; set; }
    }
}