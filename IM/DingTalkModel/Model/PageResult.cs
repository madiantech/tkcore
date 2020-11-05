using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    public class PageResult<T> : BaseResult
    {
        [SimpleElement(NamingRule = NamingRule.Camel)]
        public bool HasMore { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public int NextCursor { get; set; }

        [ObjectElement(NamingRule = NamingRule.Camel, IsMultiple = true)]
        public List<T> List { get; set; }
    }
}