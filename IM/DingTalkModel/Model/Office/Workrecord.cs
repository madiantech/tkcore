using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Office
{
    public class WorkRecord : BaseResult
    {
        [SimpleElement(NamingRule = NamingRule.Lower)]
        public string UserId { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string RecordId { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        [TkTypeConverter(typeof(IMDateTimeConverter))]
        public DateTime CreateTime { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Title { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Url { get; set; }

        [ObjectElement(NamingRule = NamingRule.Camel, IsMultiple = true)]
        public List<Form> FormItemList { get; set; }

        [ObjectElement(NamingRule = NamingRule.Camel, IsMultiple = true)]
        public List<Form> Forms { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}