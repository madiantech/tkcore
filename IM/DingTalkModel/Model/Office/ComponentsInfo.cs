using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Office
{
    public class ComponentsInfo
    {
        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Name { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Value { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string ExtValue { get; set; }
    }
}