using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Message
{
    public class MarkdownMessage : BaseMessage
    {
        public MarkdownMessage(string title, string text)
            : this(new MarkdownConfig(title, text))
        {
        }

        public MarkdownMessage(MarkdownConfig markdown)
            : base(MessageType.Markdown)
        {
            TkDebug.AssertArgumentNull(markdown, "markdown", null);

            Markdown = markdown;
        }

        [ObjectElement(NamingRule = NamingRule.Camel)]
        public MarkdownConfig Markdown { get; set; }
    }
}