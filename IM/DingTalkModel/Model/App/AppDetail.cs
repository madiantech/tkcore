using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.App
{
    public class AppDetail : BaseResult
    {
        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string AppIcon { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public int AgentId { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string AppDesc { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public bool IsSelf { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Name { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string HomepageLink { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string PcHomepageLink { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        [TkTypeConverter(typeof(BoolIntConverter))]
        public bool AppStatus { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string OmpLink { get; set; }
    }
}