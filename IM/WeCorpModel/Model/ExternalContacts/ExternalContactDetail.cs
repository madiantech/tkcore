using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;
using YJC.Toolkit.WeCorp.Model.Company;

namespace YJC.Toolkit.WeCorp.Model.ExternalContacts
{
    public class ExternalContactDetail
    {
        [SimpleElement(LocalName = "external_userid")]
        public string ExternalUserId { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Name { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Position { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Avatar { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string CorpName { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string CorpFullName { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public int Type { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        [TkTypeConverter(typeof(EnumIntTypeConverter), UseObjectType = true)]
        public Gender Gender { get; set; }

        [SimpleElement(NamingRule = NamingRule.Lower)]
        public string UnionId { get; set; }

        [ObjectElement(NamingRule = NamingRule.UnderLineLower)]
        public ExternalProfile ExternalProfile { get; set; }
    }
}