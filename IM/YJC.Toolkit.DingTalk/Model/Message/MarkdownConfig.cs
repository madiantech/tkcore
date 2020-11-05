using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Message
{
    public class MarkdownConfig
    {
        public MarkdownConfig(string title, string text)
        {
            TkDebug.AssertArgumentNullOrEmpty(title, "title", null);
            TkDebug.AssertArgumentNullOrEmpty(text, "text", null);

            Title = title;
            Text = text;
        }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Title { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Text { get; set; }
    }
}