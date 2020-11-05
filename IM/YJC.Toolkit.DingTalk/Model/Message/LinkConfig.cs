using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Message
{
    public class LinkConfig
    {
        public LinkConfig(string messageUrl, string picUrl, string title, string text)
        {
            TkDebug.AssertArgumentNullOrEmpty(messageUrl, "messageUrl", null);
            TkDebug.AssertArgumentNullOrEmpty(picUrl, "picUrl", null);
            TkDebug.AssertArgumentNullOrEmpty(title, "title", null);
            TkDebug.AssertArgumentNullOrEmpty(text, "text", null);

            MessageUrl = messageUrl;
            PicUrl = picUrl;
            Title = title;
            Text = text;
        }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string MessageUrl { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string PicUrl { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Title { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Text { get; set; }
    }
}