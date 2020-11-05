using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Office
{
    internal class WorkRecordNew
    {
        public WorkRecordNew()
        {
        }

        public WorkRecordNew(string userId, string recordId)
        {
            UserId = userId;
            RecordId = recordId;
        }

        [SimpleElement(NamingRule = NamingRule.Lower)]
        public string UserId { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string RecordId { get; set; }
    }
}