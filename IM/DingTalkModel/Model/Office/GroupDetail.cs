using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Office
{
    public class GroupDetail
    {
        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string GroupId { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public bool IsDefault { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string GroupName { get; set; }

        [ObjectElement(NamingRule = NamingRule.UnderLineLower, IsMultiple = true)]
        public List<SelectedClass> SelectedClass { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        [TkTypeConverter(typeof(UpperCaseEnumConverter), UseObjectType = true)]
        public AttendanceType Type { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public int MemberCount { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public int DefaultClassId { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string WorkDayList { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string ClassesList { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string ManagerList { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string DeptNameList { get; set; }
    }
}