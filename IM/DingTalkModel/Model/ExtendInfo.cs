using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    public class ExtendInfo
    {
        [SimpleElement(NamingRule = NamingRule.Camel)]
        [TkTypeConverter(typeof(StringListTypeConverter))]
        public List<string> Depts { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public int MainDeptId { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string MainDeptName { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Position { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string WorkPlace { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public EmployeeType EmployeeType { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string JobNumber { get; set; }
    }
}