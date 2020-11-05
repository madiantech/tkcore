using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Company
{
    internal class RoleListParam : ListParam
    {
        public RoleListParam()
        {
        }

        public RoleListParam(int roleId, int size, int offset)
        {
            RoleId = roleId;
            Size = size;
            Offset = offset;
        }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public int RoleId { get; set; }
    }
}