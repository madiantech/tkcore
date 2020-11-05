using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    public class LeaveJobDetail
    {
        [SimpleElement(NamingRule = NamingRule.Lower)]
        public string UserId { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        [TkTypeConverter(typeof(IMDateTimeConverter))]
        public DateTime LastWorkDay { get; set; }

        [ObjectElement(NamingRule = NamingRule.UnderLineLower, IsMultiple = true)]
        public List<DeptListInfo> DeptList { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string ReasonMemo { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public LeaveJobReason ReasonType { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public PreStatus PreStatus { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string HandoverUserid { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public LeaveJobStatus Status { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string MainDeptName { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public int MainDeptId { get; set; }
    }
}