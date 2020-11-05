using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Office
{
    public class SectionsDetail
    {
        [ObjectElement(NamingRule = NamingRule.Camel, IsMultiple = true)]
        public List<TimesDetail> Times { get; set; }
    }
}