using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    public class FileInfo
    {
        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string AgentId { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Code { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string MediaId { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string SpaceId { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string FolderId { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Name { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public bool Overwrite { get; set; }
    }
}