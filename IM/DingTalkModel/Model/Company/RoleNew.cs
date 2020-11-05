using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Company
{
    internal class RoleNew
    {
        public RoleNew(string roleName, int groupId)
        {
            this.RoleName = roleName;
            this.GroupId = groupId;
        }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public int GroupId { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string RoleName { get; set; }
    }
}