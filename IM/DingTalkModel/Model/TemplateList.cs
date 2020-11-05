using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    public class TemplateList
    {
        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Name { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string IconUrl { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string ReportCode { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Url { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}