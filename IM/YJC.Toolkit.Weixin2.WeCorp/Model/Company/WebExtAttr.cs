using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeCorp.Model.Company
{
    public class WebExtAttr
    {
        [SimpleElement(NamingRule = NamingRule.Camel, Order = 10)]
        public string Url { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel, Order = 20)]
        public string Title { get; set; }
    }
}