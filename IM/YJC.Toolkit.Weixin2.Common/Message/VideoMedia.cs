using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Weixin.Message
{
    public class VideoMedia
    {
        public VideoMedia(string mediaId)
        {
            TkDebug.AssertArgumentNullOrEmpty(mediaId, "mediaId", null);

            MediaId = mediaId;
        }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        //视频媒体文件id
        public string MediaId { get; private set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        //视频消息的标题
        public string Title { get; set; }

        [SimpleElement(NamingRule = NamingRule.Lower)]
        //视频消息的描述
        public string Description { get; set; }
    }
}