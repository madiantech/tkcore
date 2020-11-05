using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Company
{
    public class ContactUpdate
    {
        [ObjectElement(NamingRule = NamingRule.Camel)]
        public ExtContactParam Contact { get; set; }
    }
}