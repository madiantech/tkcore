using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Message
{
    public class ChatIdInfo : BaseResult
    {
        [SimpleElement(NamingRule = NamingRule.Lower)]
        public string ChatId { get; private set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public int ConversationTag { get; private set; }
    }
}