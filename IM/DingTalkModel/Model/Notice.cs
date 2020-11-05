using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    public class Notice : BaseResult
    {
        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string GmtCreate { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Title { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Url { get; set; }
    }
}