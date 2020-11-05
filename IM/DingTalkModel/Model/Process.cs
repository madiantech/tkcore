using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    public class Process : BaseResult
    {
        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string ProcessCode { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Description { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Name { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string BizCategoryId { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower, IsMultiple = true)]
        public List<string> FormContent { get; set; }
    }
}