using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Office
{
    public class SelectedClass
    {
        [ObjectElement(NamingRule = NamingRule.Camel, IsMultiple = true)]
        public List<SettingInfo> Setting { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string ClassId { get; set; }

        [ObjectElement(NamingRule = NamingRule.Camel, IsMultiple = true)]
        public List<SectionsDetail> Sections { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string ClassName { get; set; }
    }
}