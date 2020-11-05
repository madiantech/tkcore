using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeCorp.Model.Message
{
    public class MpNewsArticle
    {
        public MpNewsArticle(string title, string thumbMediaId, string content)
        {
            TkDebug.AssertArgumentNullOrEmpty(title, nameof(title), null);
            TkDebug.AssertArgumentNullOrEmpty(thumbMediaId, nameof(thumbMediaId), null);
            TkDebug.AssertArgumentNullOrEmpty(content, nameof(content), null);

            Title = title;
            ThumbMediaId = thumbMediaId;
            Content = content;
        }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        //视频消息的标题
        public string Title { get; private set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        //图文消息缩略图的media_id, 可以通过素材管理接口获得。此处thumb_media_id即上传接口返回的media_id
        public string ThumbMediaId { get; private set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        //图文消息的作者，不超过64个字节
        public string Author { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        //图文消息点击“阅读原文”之后的页面链接
        public string ContentSourceUrl { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        //图文消息的内容，支持html标签，不超过666 K个字节
        public string Content { get; private set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        //图文消息的描述，不超过512个字节，超过会自动截断
        public string Digest { get; set; }
    }
}