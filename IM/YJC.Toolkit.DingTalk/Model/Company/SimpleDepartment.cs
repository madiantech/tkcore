using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Company
{
    public class SimpleDepartment : BaseResult
    {
        public SimpleDepartment()
        {
        }

        public SimpleDepartment(string name, string parentId, bool createDeptGroup, bool autoAddUser)
        {
            Name = name;
            ParentId = parentId;
            CreateDeptGroup = createDeptGroup;
            AutoAddUser = autoAddUser;
        }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Id { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Name { get; set; }

        [SimpleElement(NamingRule = NamingRule.Lower)]
        public string ParentId { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public bool CreateDeptGroup { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public bool AutoAddUser { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Name) ? base.ToString() : Name;
        }
    }
}