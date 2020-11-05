using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Message
{
    public class OaBodyForm
    {
        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Key { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Value { get; set; }
    }
}