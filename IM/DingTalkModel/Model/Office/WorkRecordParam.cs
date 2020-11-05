using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Office
{
    public class WorkRecordParam
    {
        public const int LIMITMAX = 50;

        internal WorkRecordParam()
        {
        }

        public WorkRecordParam(string userId, int offset, int limit, bool status)
        {
            TkDebug.AssertArgument(offset >= 0, "offset",
                string.Format(ObjectUtil.SysCulture, "offset参数必须>0，当前值为{0}", offset), null);
            TkDebug.AssertArgument(limit > 0 && limit <= LIMITMAX, "limit",
                string.Format(ObjectUtil.SysCulture, "limit参数必须在0~50之间,当前值为{0}", limit), null);

            UserId = userId;
            Offset = offset;
            Limit = limit;
            Status = status;
        }

        [SimpleElement(NamingRule = NamingRule.Lower)]
        public string UserId { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public int Offset { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public int Limit { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        [TkTypeConverter(typeof(BoolIntConverter))]
        public bool Status { get; set; }
    }
}