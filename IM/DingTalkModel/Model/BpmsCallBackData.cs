using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    public class BpmsCallBackData
    {
        [SimpleElement(NamingRule = NamingRule.Camel)]
        public List<string> BpmsCallbackData { get; set; }
    }
}