using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeCorp.Model.ExternalContacts
{
    public class ExtContactResult:BaseResult
    {
        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public ExternalContactDetail ExternalContact { get; set; }

        [ObjectElement(NamingRule = NamingRule.UnderLineLower, IsMultiple = true)]
        public List<FollowUser> FollowUser { get; set; }
    }
}