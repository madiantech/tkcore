using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Company
{
    public class Department : SimpleDepartment
    {
        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Order { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public bool DeptHiding { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        [TkTypeConverter(typeof(StringListTypeConverter))]
        public List<string> DeptPerimits { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        [TkTypeConverter(typeof(StringListTypeConverter))]
        public List<string> UserPerimits { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public bool OuterDept { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        [TkTypeConverter(typeof(StringListTypeConverter))]
        public List<string> OuterPermitDepts { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        [TkTypeConverter(typeof(StringListTypeConverter))]
        public List<string> OuterPermitUsers { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string OrgDeptOwner { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        [TkTypeConverter(typeof(StringListTypeConverter))]
        public List<string> DeptManagerUseridList { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string SourceIdentifier { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public bool GroupContainSubDept { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public bool OuterDeptOnlySelf { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public bool GroupContainOuterDept { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public bool GroupContainHiddenDept { get; set; }
    }
}