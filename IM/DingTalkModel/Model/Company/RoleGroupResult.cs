using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Company
{
    public class RoleGroupResult : BaseResult
    {
        [ObjectElement(NamingRule = NamingRule.Camel, IsMultiple = true)]
        public List<RolesDetail> Roles { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string GroupName { get; set; }

        public override string ToString()
        {
            return GroupName;
        }
    }
}