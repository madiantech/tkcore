using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Office
{
    public class LeaveStatusResult : BaseResult
    {
        [ObjectElement(NamingRule = NamingRule.Camel)]
        public PageResult<StatusDetail> Result { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public bool Success { get; set; }
    }
}