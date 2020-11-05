using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Company
{
    public class ExtContact : BaseResult
    {
        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Title { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower, IsMultiple = true)]
        public List<int> ShareDeptIds { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower, IsMultiple = true)]
        public List<int> LabelIds { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Remark { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Address { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Name { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string FollowerUserId { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string StateCode { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string CompanyName { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower, IsMultiple = true)]
        public List<string> ShareUserIds { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Mobile { get; set; }

        [SimpleElement(NamingRule = NamingRule.Lower)]
        public string UserId { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}