using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Company
{
    internal class ExtDetailParam
    {
        public ExtDetailParam()
        {
        }

        public ExtDetailParam(string userId)
        {
            this.UserId = userId;
        }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string UserId { get; set; }
    }
}