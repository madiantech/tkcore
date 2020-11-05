using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeCorp.Model.Company
{
    //成员管理（邀请成员）
    internal class UserInviteParam
    {
        public UserInviteParam(List<string> user, List<int> party, List<int> tag)
        {
            TkDebug.Assert(user != null || party != null || tag != null,
                $"{nameof(user)}、{nameof(party)}和{nameof(tag)}不能同时为空", null);

            User = user;
            Party = party;
            Tag = tag;
        }

        [SimpleElement(NamingRule = NamingRule.Camel, IsMultiple = true)]
        public List<string> User { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel, IsMultiple = true)]
        public List<int> Party { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel, IsMultiple = true)]
        public List<int> Tag { get; set; }
    }
}