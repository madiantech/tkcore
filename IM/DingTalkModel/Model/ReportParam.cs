using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    public class ReportParam
    {
        public ReportParam()
        {
        }

        public ReportParam(DateTime startTime, DateTime endTime, string tempName, int cursor, int size)
        {
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.TemplateName = tempName;
            this.Cursor = cursor;
            this.Size = size;
        }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        [TkTypeConverter(typeof(IMDateTimeConverter))]
        public DateTime StartTime { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        [TkTypeConverter(typeof(IMDateTimeConverter))]
        public DateTime EndTime { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string TemplateName { get; set; }

        [SimpleElement(NamingRule = NamingRule.Lower)]
        public string UserId { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public int Cursor { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public int Size { get; set; }

        public override string ToString()
        {
            return TemplateName;
        }
    }
}