using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class PageUrlInfo
    {
        [SimpleAttribute(NamingRule = NamingRule.Camel)]
        public string Title { get; set; }

        [SimpleAttribute(NamingRule = NamingRule.Camel)]
        public string Width { get; set; }

        [ObjectElement(NamingRule = NamingRule.Camel)]
        public PageTemplateInfo Query { get; set; }
    }
}