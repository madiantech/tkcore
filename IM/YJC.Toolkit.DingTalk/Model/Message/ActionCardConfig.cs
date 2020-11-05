using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Message
{
    public class ActionCardConfig
    {
        public ActionCardConfig(string title, string markdown, string singleTitle, string singleUrl)
        {
            TkDebug.AssertArgumentNullOrEmpty(title, "title", null);
            TkDebug.AssertArgumentNullOrEmpty(markdown, "markdown", null);
            TkDebug.AssertArgumentNullOrEmpty(singleTitle, "singleTitle", null);
            TkDebug.AssertArgumentNullOrEmpty(singleUrl, "singleUrl", null);

            Title = title;
            Markdown = markdown;
            SingleTitle = singleTitle;
            SingleUrl = singleUrl;
        }

        public ActionCardConfig(string title, string markdown, Orientation orientation,
            List<ActionButton> btnJsonList)
        {
            TkDebug.AssertArgumentNullOrEmpty(title, "title", null);
            TkDebug.AssertArgumentNullOrEmpty(markdown, "markdown", null);
            TkDebug.AssertArgumentNull(btnJsonList, "btnJsonList", null);

            Title = title;
            Markdown = markdown;
            BtnOrientation = orientation;
            BtnJsonList = btnJsonList;
        }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Title { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Markdown { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string SingleTitle { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string SingleUrl { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        [TkTypeConverter(typeof(EnumIntTypeConverter), UseObjectType = true)]
        public Orientation BtnOrientation { get; set; }

        [ObjectElement(NamingRule = NamingRule.UnderLineLower, IsMultiple = true)]
        public List<ActionButton> BtnJsonList { get; set; }
    }
}