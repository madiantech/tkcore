using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Office
{
    public class AttendanceParam
    {
        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string WorkDateFrom { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string WorkDateTo { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel, IsMultiple = true)]
        public List<string> UserIdList { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public long Offset { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public long Limit { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public bool IsI18n { get; set; }
    }
}