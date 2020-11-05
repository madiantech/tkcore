using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Office
{
    internal class NoticeDetailParam : ListParam
    {
        public NoticeDetailParam()
        {
        }

        public NoticeDetailParam(DateTime workDate, int size, int offset)
        {
            WorkDate = workDate;
            Size = size;
            Offset = offset;
        }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public DateTime WorkDate { get; set; }
    }
}