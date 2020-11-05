using System.Collections.Generic;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Office
{
    public class CheckinRecord : BaseResult
    {
        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Name { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string UserId { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        [TkTypeConverter(typeof(IMDateTimeConverter))]
        public string Timestamp { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Avatar { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Place { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string DetailPlace { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Remark { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public float Latitude { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public float Longitude { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel, IsMultiple = true)]
        public List<string> ImageList { get; set; }
    }
}