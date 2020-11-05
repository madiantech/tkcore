using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    public class ReportResult : BaseResult
    {
        [ObjectElement(NamingRule = NamingRule.UnderLineLower, IsMultiple = true)]
        public List<ReportContent> DataList { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public int Size { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public int NextCursor { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public bool HasMore { get; set; }
    }
}