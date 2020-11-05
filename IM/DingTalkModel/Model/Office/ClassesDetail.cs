using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Office
{
    public class ClassesDetail
    {
        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string ClassId { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Name { get; set; }

        [ObjectElement(NamingRule = NamingRule.Camel, IsMultiple = true)]
        public List<ClassSectionsDetail> Sections { get; set; }

        [ObjectElement(NamingRule = NamingRule.Camel)]
        public SettingDetail Setting { get; set; }
    }
}