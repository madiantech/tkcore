using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Office
{
    public class SettingDetail
    {
        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public RestTimeDetail RestBeginTime { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public RestTimeDetail RestEndTime { get; set; }
    }
}