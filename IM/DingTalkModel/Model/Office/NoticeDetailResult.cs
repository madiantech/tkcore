using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Office
{
    public class NoticeDetailResult : BaseResult
    {
        [ObjectElement(NamingRule = NamingRule.Camel, IsMultiple = true)]
        public List<SchedulesDetail> Schedules { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public bool HasMore { get; set; }
    }
}