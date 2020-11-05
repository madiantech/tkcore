using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    public class SingleProcessDetail : BaseResult
    {
        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Title { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string CreateTime { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string FinishTime { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string OriginatorUserid { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string OriginatorDeptId { get; set; }
    }
}