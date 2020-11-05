using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    public class ReportContentDetail
    {
        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Sort { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Type { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Value { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Key { get; set; }
    }
}