using YJC.Toolkit.Sys;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeCorp.Model.Message
{
    public class VideoChatMessage : BaseChatMessage
    {
        public VideoChatMessage(string chatId, VideoMedia video)
            : base(chatId, MessageType.Video)
        {
            TkDebug.AssertArgumentNull(video, nameof(video), null);

            Video = video;
        }

        [ObjectElement(NamingRule = NamingRule.Camel, Order = 40)]//媒体文件
        public VideoMedia Video { get; private set; }

        //表示是否是保密消息，0表示否，1表示是，默认0
        [SimpleElement(NamingRule = NamingRule.Camel, Order = 100)]
        [TkTypeConverter(typeof(BoolIntConverter))]
        public bool Safe { get; set; }
    }
}