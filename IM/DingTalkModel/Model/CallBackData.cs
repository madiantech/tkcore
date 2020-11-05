using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    public class CallBackData
    {
        [SimpleElement(NamingRule = NamingRule.Camel)]
        public List<string> CallbackData { get; set; }
    }
}