using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeCorp.Model.Company
{
    public class MiniProgramExtAttr
    {
        [SimpleElement(NamingRule = NamingRule.Lower, Order = 10)]
        public string AppId { get; set; }

        [SimpleElement(NamingRule = NamingRule.Lower, Order = 20)]
        public string PagePath { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel, Order = 30)]
        public string Title { get; set; }
    }
}