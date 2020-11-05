using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeCorp.Model
{
    //异步批量接口（增量更新成员）
    public class SyncUser
    {
        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string MediaId { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public bool ToInvite { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public CallBack Callback { get; set; }
    }
}