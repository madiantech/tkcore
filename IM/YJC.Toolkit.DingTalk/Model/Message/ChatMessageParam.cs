using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Message
{
    internal class ChatMessageParam : BaseResult
    {
        public ChatMessageParam()
        {
        }

        public ChatMessageParam(string chatId, BaseMessage msg)
        {
            ChatId = chatId;
            Msg = msg;
        }

        [SimpleElement(NamingRule = NamingRule.Lower)]
        public string ChatId { get; set; }

        [ObjectElement(NamingRule = NamingRule.Camel)]
        public BaseMessage Msg { get; set; }
    }
}