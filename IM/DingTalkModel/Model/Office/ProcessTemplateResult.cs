using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Office
{
    public class ProcessTemplateResult : BaseResult
    {
        [ObjectElement(NamingRule = NamingRule.UnderLineLower, IsMultiple = true)]
        public List<ProcessInfo> ProcessList { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public int NextCursor { get; set; }
    }
}