using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeCorp.Model
{
    public class ResultDetail : BaseResult
    {
        [SimpleElement(NamingRule = NamingRule.Lower)]
        public string UserId { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public int Action { get; set; }

        [SimpleElement(NamingRule = NamingRule.Lower)]
        public int PartyId { get; set; }
    }
}