using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    internal class StepInfoParam
    {
        public StepInfoParam()
        {
        }

        public StepInfoParam(int type, string objectId, string statDates)
        {
            Type = type;
            ObjectId = objectId;
            StatDates = statDates;
        }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public int Type { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string ObjectId { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string StatDates { get; set; }
    }
}