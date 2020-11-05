using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Office
{
    public class ProcessDetail
    {
        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public int AgentId { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string ProcessCode { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string OriginatorUserId { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public int DeptId { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Approvers { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string CcList { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        [TkTypeConverter(typeof(EnumFieldValueTypeConverter), UseObjectType = true)]
        public CcPosition CcPosition { get; set; }

        [ObjectElement(NamingRule = NamingRule.UnderLineLower, IsMultiple = true)]
        public List<ComponentsInfo> FormComponentValues { get; set; }
    }
}