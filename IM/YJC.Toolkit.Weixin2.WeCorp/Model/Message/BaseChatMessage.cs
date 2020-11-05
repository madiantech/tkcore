using YJC.Toolkit.Sys;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeCorp.Model.Message
{
    public class BaseChatMessage : BaseMessage
    {
        public BaseChatMessage(string chatId, MessageType messageType)
        {
            TkDebug.AssertArgumentNullOrEmpty(chatId, nameof(chatId), null);

            MsgType = messageType;
            ChatId = chatId;
        }

        [SimpleElement(NamingRule = NamingRule.Lower, Order = 10)]//群聊id
        public string ChatId { get; private set; }
    }
}