using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Office
{
    public class RestTimeDetail
    {
        public RestTimeDetail(string checkTime)
        {
            CheckTime = checkTime;
        }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public int Across { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public int BeginMin { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public int EndMin { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string CheckTime { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public CheckType CheckType { get; set; }
    }
}