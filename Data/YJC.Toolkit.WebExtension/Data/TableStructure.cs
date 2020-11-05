using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class TableStructure
    {
        [SimpleAttribute(NamingRule = NamingRule.Camel)]
        public string Structure { get; set; }
    }
}