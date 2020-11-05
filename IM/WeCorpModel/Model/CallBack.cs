using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeCorp.Model
{
    public class CallBack
    {
        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Url { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Token { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Encodingaeskey { get; set; }
    }
}