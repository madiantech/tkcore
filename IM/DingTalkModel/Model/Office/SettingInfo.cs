using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Office
{
    public class SettingInfo
    {
        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public int ClassSettingId { get; set; }

        [ObjectElement(NamingRule = NamingRule.UnderLineLower)]
        public RestTimeDetail RestBeginTime { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public int PermitLateMinutes { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public int WorkTimeMinutes { get; set; }

        [ObjectElement(NamingRule = NamingRule.UnderLineLower)]
        public RestTimeDetail RestEndTime { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public int AbsenteeismLateMinutes { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public int SeriousLateMinutes { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public IsOffDutyFreeCheck IsOffDutyFreeCheck { get; set; }
    }
}