using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    public class StepInfoResult : BaseResult
    {
        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string StatDate { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public int StepCount { get; set; }

        [SimpleElement(NamingRule = NamingRule.Lower)]
        public string UserId { get; set; }
    }
}