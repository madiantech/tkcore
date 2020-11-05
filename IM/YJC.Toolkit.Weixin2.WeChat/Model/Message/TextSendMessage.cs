using YJC.Toolkit.Sys;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeChat.Model.Message
{
    public class TextSendMessage : BaseSendMessage
    {
        public TextSendMessage(string toUser, string fromUser, string content)
            : base(toUser, fromUser, MessageType.Text)
        {
            TkDebug.AssertArgumentNull(content, "content", null);

            Content = content;
        }

        public TextSendMessage(ReceiveMessage receive, string content)
            : base(receive, MessageType.Text)
        {
            TkDebug.AssertArgumentNull(content, "content", null);

            Content = content;
        }

        [SimpleElement(Order = 50, UseCData = true)]
        public string Content { get; private set; }
    }
}