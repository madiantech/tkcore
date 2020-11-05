using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeCorp.Model.Application
{
    public class AllowTag
    {
        [SimpleElement(NamingRule = NamingRule.Lower)]
        public List<int> TagId { get; set; }
    }
}