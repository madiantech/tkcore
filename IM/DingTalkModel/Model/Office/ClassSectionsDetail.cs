using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Office
{
    public class ClassSectionsDetail
    {
        [ObjectElement(NamingRule = NamingRule.Camel, IsMultiple = true)]
        public List<SectionsTimesDetail> Times { get; set; }
    }
}