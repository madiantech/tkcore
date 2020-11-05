using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    public class WaitResultDetail
    {
        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public int NextCursor { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower, IsMultiple = true)]
        public List<string> DataList { get; set; }
    }
}