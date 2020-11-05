using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeCorp.Model.Company
{
    //邀请成员
    public class UserInviteResult : BaseResult
    {
        [SimpleElement(NamingRule = NamingRule.Lower, IsMultiple = true)]
        public List<string> InvalidUser { get; set; }

        [SimpleElement(NamingRule = NamingRule.Lower, IsMultiple = true)]
        public List<int> InvalidParty { get; set; }

        [SimpleElement(NamingRule = NamingRule.Lower, IsMultiple = true)]
        public List<int> InvalidTag { get; set; }
    }
}