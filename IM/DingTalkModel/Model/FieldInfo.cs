using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    public class FieldInfo
    {
        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string FieldName { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string FieldCode { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string GroupId { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Label { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Value { get; set; }
    }
}