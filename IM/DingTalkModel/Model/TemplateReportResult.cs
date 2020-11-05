using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    public class TemplateReportResult : BaseResult
    {
        [ObjectElement(NamingRule = NamingRule.UnderLineLower, IsMultiple = true)]
        public List<TemplateList> TemplateList { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public int NextCursor { get; set; }
    }
}