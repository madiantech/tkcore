using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Company
{
    public class RolesDetail
    {
        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public int RoleId { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string RoleName { get; set; }
    }
}