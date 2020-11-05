using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Message
{
    public class OaConfig
    {
        public OaConfig(string messageUrl, string pcMessageUrl, OaHead head, OaBody body)
        {
            TkDebug.AssertArgumentNullOrEmpty(messageUrl, "messageUrl", null);
            TkDebug.AssertArgumentNull(head, "head", null);
            TkDebug.AssertArgumentNull(body, "body", null);

            MessageUrl = messageUrl;
            PcMesssageUrl = pcMessageUrl;
            Head = head;
            Body = body;
        }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string MessageUrl { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string PcMesssageUrl { get; set; }

        [ObjectElement(NamingRule = NamingRule.Camel)]
        public OaHead Head { get; set; }

        [ObjectElement(NamingRule = NamingRule.Camel)]
        public OaBody Body { get; set; }
    }
}