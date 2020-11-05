using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Company
{
    internal class RolesParam
    {
        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string RoleIds { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string UserIds { get; set; }
    }
}