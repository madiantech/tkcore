using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Company
{
    internal class RoleGroupParam
    {
        public RoleGroupParam()
        {
        }

        public RoleGroupParam(string name)
        {
            this.Name = name;
        }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Name { get; set; }
    }
}