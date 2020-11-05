using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Message
{
    public class TextMessage : BaseMessage
    {
        public TextMessage(string text)
            : base(MessageType.Text)
        {
            TkDebug.AssertArgumentNullOrEmpty(text, "text", null);

            Text = text;
        }

        [TagElement(NamingRule = NamingRule.Camel)]
        [SimpleElement(LocalName = "content")]
        public string Text { get; private set; }
    }
}