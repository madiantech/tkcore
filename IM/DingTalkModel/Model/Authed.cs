using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    public class Authed : BaseResult
    {
        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public List<int> AuthedDept { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public List<string> AuthedUser { get; set; }
    }
}