using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    public class DeptListInfo
    {
        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string DeptPath { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public int DeptId { get; set; }
    }
}