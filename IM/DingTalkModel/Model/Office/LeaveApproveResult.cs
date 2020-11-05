using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Office
{
    public class LeaveApproveResult : BaseResult
    {
        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public int DurationInMinutes { get; set; }
    }
}