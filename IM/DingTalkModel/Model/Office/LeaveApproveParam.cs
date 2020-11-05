using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Office
{
    internal class LeaveApproveParam
    {
        public LeaveApproveParam()
        {
        }

        public LeaveApproveParam(string userId, DateTime fromDate, DateTime toDate)
        {
            UserId = userId;
            FromDate = fromDate;
            ToDate = toDate;
        }

        [SimpleElement(NamingRule = NamingRule.Lower)]
        public string UserId { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public DateTime FromDate { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public DateTime ToDate { get; set; }
    }
}