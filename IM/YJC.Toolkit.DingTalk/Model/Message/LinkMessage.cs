using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Message
{
    public class LinkMessage : BaseMessage
    {
        public LinkMessage(string messageUrl, string picUrl, string title, string text)
            : this(new LinkConfig(messageUrl, picUrl, title, text))
        {
        }

        public LinkMessage(LinkConfig link)
            : base(MessageType.Link)
        {
            TkDebug.AssertArgumentNull(link, "link", null);

            Link = link;
        }

        [ObjectElement(NamingRule = NamingRule.Camel)]
        public LinkConfig Link { get; set; }
    }
}