using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Office
{
    public class ProcessInfo
    {
        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Name { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string IconUrl { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string ProcessCode { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Url { get; set; }
    }
}