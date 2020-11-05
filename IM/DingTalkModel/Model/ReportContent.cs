using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    public class ReportContent
    {
        [ObjectElement(NamingRule = NamingRule.Camel, IsMultiple = true)]
        public List<ReportContentDetail> Contents { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Remark { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string TemplateName { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string DeptName { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string CreatorName { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string CreatorId { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        [TkTypeConverter(typeof(IMDateTimeConverter))]
        public DateTime CreateTime { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string ReportId { get; set; }
    }
}