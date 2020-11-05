using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeCorp.Model.ExternalContacts
{
    public class FollowUser
    {
        [SimpleElement(NamingRule = NamingRule.Lower)]
        public string UserId { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Remark { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Description { get; set; }

        [SimpleElement(NamingRule = NamingRule.Lower)]
        [TkTypeConverter(typeof(IMDateTimeConverter))]
        public DateTime CreateTime { get; set; }
    }
}