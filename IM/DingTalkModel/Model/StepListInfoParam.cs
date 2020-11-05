using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    internal class StepListInfoParam
    {
        public StepListInfoParam()
        {
        }

        public StepListInfoParam(string userIds, string statDate)
        {
            UserIds = userIds;
            StatDate = statDate;
        }

        [SimpleElement(NamingRule = NamingRule.Lower)]
        public string UserIds { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string StatDate { get; set; }
    }
}