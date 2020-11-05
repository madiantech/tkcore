using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Weixin.Message
{
    public class NewsArticle
    {
        public NewsArticle(string title, string url)
        {
            TkDebug.AssertArgumentNullOrEmpty(title, "title", null);
            TkDebug.AssertArgumentNullOrEmpty(url, "url", null);

            Title = title;
            Url = url;
        }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        //视频消息的标题
        public string Title { get; protected set; }

        [SimpleElement(NamingRule = NamingRule.Lower)]
        //视频消息的描述
        public string Description { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        //点击后跳转的链接
        public string Url { get; protected set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        //图文消息的图片链接
        public string PicUrl { get; set; }
    }
}