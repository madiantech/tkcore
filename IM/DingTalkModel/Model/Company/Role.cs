using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Company
{
    internal class Role
    {
        public Role(int roleId)
        {
            RoleId = roleId;
        }

        public Role(string roleName, int roleId)
        {
            RoleName = roleName;
            RoleId = roleId;
        }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string RoleName { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public int RoleId { get; set; }
    }
}