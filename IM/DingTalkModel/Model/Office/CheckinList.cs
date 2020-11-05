using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Office
{
    public class CheckinList
    {
        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string CheckinTime { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string ImageList { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string DetailPlace { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Remark { get; set; }

        [SimpleElement(NamingRule = NamingRule.Lower)]
        public string UserId { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Place { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Longitude { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Latitude { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string VisitUser { get; set; }
    }
}