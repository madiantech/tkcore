using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    public class CorpResult<T>
    {
        [SimpleElement(NamingRule = NamingRule.Lower)]
        public string CorpId { get; set; }

        [ObjectElement(NamingRule = NamingRule.Camel, IsMultiple = true)]
        public List<T> List { get; set; }
    }
}