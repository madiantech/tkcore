using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Office
{
    public class TasksInfo
    {
        [SimpleElement(NamingRule = NamingRule.Lower)]
        public string UserId { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        [TkTypeConverter(typeof(UpperCaseEnumConverter), UseObjectType = true)]
        public TaskStatus TaskStatus { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        [TkTypeConverter(typeof(UpperCaseEnumConverter), UseObjectType = true)]
        public TaskResult TaskResult { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string CreateTime { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string FinishTime { get; set; }

        [SimpleElement(NamingRule = NamingRule.Lower)]
        public string TaskId { get; set; }
    }
}