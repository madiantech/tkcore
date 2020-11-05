using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Office
{
    public class ListRecordParam
    {
        [SimpleElement(NamingRule = NamingRule.Camel, IsMultiple = true)]
        public List<string> UserIds { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string CheckDateFrom { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string CheckDateTo { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public bool IsI18n { get; set; }
    }
}