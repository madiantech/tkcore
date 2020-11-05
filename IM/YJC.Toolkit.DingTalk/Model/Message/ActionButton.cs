using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Message
{
    public class ActionButton
    {
        public ActionButton(string title, string actionUrl)
        {
            TkDebug.AssertArgumentNullOrEmpty(title, "title", null);
            TkDebug.AssertArgumentNullOrEmpty(actionUrl, "actionUrl", null);

            Title = title;
            ActionUrl = actionUrl;
        }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Title { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string ActionUrl { get; set; }
    }
}