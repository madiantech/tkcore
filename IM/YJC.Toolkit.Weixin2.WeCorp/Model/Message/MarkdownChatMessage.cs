using YJC.Toolkit.Sys;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeCorp.Model.Message
{
    public class MarkdownChatMessage : BaseChatMessage
    {
        public MarkdownChatMessage(string chatId, string content)
            : base(chatId, MessageType.Markdown)
        {
            TkDebug.AssertArgumentNullOrEmpty(content, nameof(content), null);

            Content = content;
        }

        [TagElement(LocalName = "markdown", Order = 40)]
        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Content { get; private set; }
    }
}