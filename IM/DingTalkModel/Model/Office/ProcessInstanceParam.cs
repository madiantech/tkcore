using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Office
{
    public class ProcessInstanceParam
    {
        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string ProcessCode { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        [TkTypeConverter(typeof(IMDateTimeConverter))]
        public DateTime StartTime { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        [TkTypeConverter(typeof(IMDateTimeConverter))]
        public DateTime EndTime { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public int Size { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public int Cursor { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public List<string> UseridList { get; set; }
    }
}