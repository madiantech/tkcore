using YJC.Toolkit.Sys;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeCorp.Model.Message
{
    public class TextChatMessage : BaseChatMessage
    {
        public TextChatMessage(string chatId, string content)
            : base(chatId, MessageType.Text)
        {
            TkDebug.AssertArgumentNullOrEmpty(content, nameof(content), null);

            Content = content;
        }

        [TagElement(LocalName = "text", Order = 40)]
        [SimpleElement(NamingRule = NamingRule.Camel)]
        //消息内容，最长不超过2048个字节
        public string Content { get; private set; }

        //表示是否是保密消息，0表示否，1表示是，默认0
        [SimpleElement(NamingRule = NamingRule.Camel, Order = 100)]
        [TkTypeConverter(typeof(BoolIntConverter))]
        public bool Safe { get; set; }
    }
}