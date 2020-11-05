using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeCorp.Model.Application
{
    public class AppDetail : BaseResult
    {
        public AppDetail(string agentId, string name, string squareLogoUrl)
        {
            AgentId = agentId;
            Name = name;
            SquareLogoUrl = squareLogoUrl;
        }

        public AppDetail(string agentId, bool reportLocationFlag, string logoMediaId,
            string name, string description, string redirectDomain, bool isReportEnter, string homeUrl)
        {
            AgentId = agentId;
            ReportLocationFlag = reportLocationFlag;
            LogoMediaId = logoMediaId;
            Name = name;
            Description = description;
            RedirectDomain = redirectDomain;
            IsReportEnter = isReportEnter;
            HomeUrl = homeUrl;
        }

        [SimpleElement(NamingRule = NamingRule.Lower)]
        public string AgentId { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Name { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string SquareLogoUrl { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Description { get; set; }

        [ObjectElement(LocalName = "allow_userinfos")]
        public AllowUserInfo AllowUserInfos { get; set; }

        [ObjectElement(NamingRule = NamingRule.UnderLineLower)]
        public AllowParty AllowPartys { get; set; }

        [ObjectElement(NamingRule = NamingRule.UnderLineLower)]
        public AllowTag AllowTags { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        [TkTypeConverter(typeof(BoolIntConverter))]
        public bool Close { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string RedirectDomain { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        [TkTypeConverter(typeof(BoolIntConverter))]
        public bool ReportLocationFlag { get; set; }

        [SimpleElement(NamingRule = NamingRule.Lower)]
        [TkTypeConverter(typeof(BoolIntConverter))]
        public bool IsReportEnter { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string HomeUrl { get; set; }

        [SimpleElement(LocalName = "logo_mediaid")]
        public string LogoMediaId { get; set; }
    }
}