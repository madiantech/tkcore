using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class PageTemplateInfo
    {
        [SimpleAttribute(NamingRule = NamingRule.Camel)]
        public string PageType { get; set; }

        [SimpleAttribute(NamingRule = NamingRule.Camel)]
        public string DataUrl { get; set; }

        [SimpleAttribute(NamingRule = NamingRule.Camel)]
        public string TempUrl { get; set; }

        [ObjectElement(NamingRule = NamingRule.Camel)]
        public TableStructure DataConf { get; set; }
    }
}