using System;
using System.Collections.Generic;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Company
{
    public class User : SimpleUser
    {
        public User()
        {
        }

        public User(string userId, string name)
            : base(userId, name)
        {
        }

        [SimpleElement(NamingRule = NamingRule.Lower)]
        public string UnionId { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Tel { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string WorkPlace { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Remark { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Mobile { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Email { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string OrgEmail { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public bool Active { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string OrderInDepts { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public bool IsAdmin { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public bool IsBoss { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string IsLeaderInDepts { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public bool IsHide { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel, IsMultiple = true)]
        public List<int> Department { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Position { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Avatar { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        [TkTypeConverter(typeof(IMDateTimeConverter))]
        public DateTime HiredDate { get; set; }

        [SimpleElement(NamingRule = NamingRule.Lower)]
        public string JobNumber { get; set; }

        [Dictionary(NamingRule = NamingRule.Lower)]
        public Dictionary<string, string> ExtAttr { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public bool IsSenior { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public bool IsLeader { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public int Order { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string StateCode { get; set; }

        //[SimpleElement(NamingRule = NamingRule.Camel)]
        //public string OpenId { get; set; }

        //[ObjectElement(NamingRule = NamingRule.Camel, IsMultiple = true)]
        //public List<UserRoleInfo> Roles { get; set; }
    }
}