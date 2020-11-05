using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Office
{
    public class ListRecordResult : BaseResult
    {
        [SimpleElement(NamingRule = NamingRule.Camel)]
        [TkTypeConverter(typeof(IMDateTimeConverter))]
        public DateTime GmtModified { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string IsLegal { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        [TkTypeConverter(typeof(IMDateTimeConverter))]
        public DateTime BaseCheckTime { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Id { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string UserAddress { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string UserId { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public CheckType CheckType { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public TimeResult TimeResult { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string DeviceId { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string CorpId { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        [TkTypeConverter(typeof(EnumFieldValueTypeConverter), UseObjectType = true)]
        public SourceType SourceType { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        [TkTypeConverter(typeof(IMDateTimeConverter))]
        public DateTime WorkDate { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        [TkTypeConverter(typeof(IMDateTimeConverter))]
        public DateTime PlanCheckTime { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        [TkTypeConverter(typeof(IMDateTimeConverter))]
        public DateTime GmtCreate { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string LocationMethod { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public LocationResult LocationResult { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public float UserLongitude { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string PlanId { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string GroupId { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public int UserAccuracy { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        [TkTypeConverter(typeof(IMDateTimeConverter))]
        public DateTime UserCheckTime { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public float UserLatitude { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string ProcInstId { get; set; }
    }
}