using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Company
{
    public class ExtContactLabel : BaseResult
    {
        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Id { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}