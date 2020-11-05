using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeCorp.Model.Application
{
    public class AllowParty
    {
        [SimpleElement(NamingRule = NamingRule.Lower)]
        public List<int> PartyId { get; set; }
    }
}