using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeCorp.Model.Message
{
    public class ChatInfo
    {
        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Name { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Owner { get; set; }

        [SimpleElement(NamingRule = NamingRule.Lower, IsMultiple = true)]
        public List<string> UserList { get; set; }

        [SimpleElement(NamingRule = NamingRule.Lower)]
        public string ChatId { get; set; }
    }
}