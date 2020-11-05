using System.Drawing;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Message
{
    public class OaHead
    {
        public OaHead(Color bgColor, string text)
        {
            TkDebug.AssertArgumentNullOrEmpty(text, "text", null);

            BgColor = bgColor;
            Text = text;
        }

        [SimpleElement(NamingRule = NamingRule.Lower)]
        [TkTypeConverter(typeof(ColorTypeConverter))]
        public Color BgColor { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Text { get; set; }
    }
}