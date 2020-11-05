using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    public class CallbackDetail : BaseResult
    {
        [SimpleElement(NamingRule = NamingRule.UnderLineLower, IsMultiple = true)]
        public List<string> CallBackTag { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Token { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string AesKey { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Url { get; set; }
    }
}