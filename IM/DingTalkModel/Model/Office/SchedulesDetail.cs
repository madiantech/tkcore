using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Office
{
    public class SchedulesDetail
    {
        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public int PlanId { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public CheckType CheckType { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public int ApproveId { get; set; }

        [SimpleElement(NamingRule = NamingRule.Lower)]
        public string UserId { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public int ClassId { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public int ClassSettingId { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string PlanCheckTime { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public int GroupId { get; set; }
    }
}