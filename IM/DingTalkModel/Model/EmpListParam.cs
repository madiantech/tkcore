using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    internal class EmpListParam : ListParam
    {
        public EmpListParam()
        {
        }

        public EmpListParam(string statusList, int offset, int size)
        {
            StatusList = statusList;
            Offset = offset;
            Size = size;
        }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string StatusList { get; set; }
    }
}