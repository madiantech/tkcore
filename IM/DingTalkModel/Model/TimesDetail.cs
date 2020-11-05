using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.DingTalk.Model.Office;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    public class TimesDetail
    {
        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string CheckTime { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public CheckType CheckType { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public int Across { get; set; }
    }
}