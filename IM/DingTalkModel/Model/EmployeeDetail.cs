using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    public class EmployeeDetail
    {
        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Name { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Mobile { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public DateTime PreEntryTime { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string OpUserid { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public ExtendInfo ExtendInfo { get; set; }
    }
}