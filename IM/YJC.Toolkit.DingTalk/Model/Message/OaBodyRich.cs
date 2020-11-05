using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Message
{
    public class OaBodyRich
    {
        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Num { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Unit { get; set; }
    }
}