using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Office
{
    public class ListIdsResult : BaseResult
    {
        [SimpleElement(NamingRule = NamingRule.Camel, IsMultiple = true)]
        public List<string> List { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public int NextCursor { get; set; }
    }
}