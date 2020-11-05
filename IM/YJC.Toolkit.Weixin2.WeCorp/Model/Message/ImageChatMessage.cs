using YJC.Toolkit.Sys;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeCorp.Model.Message
{
    public class ImageChatMessage : BaseChatMessage
    {
        public ImageChatMessage(string chatId, string mediaId)
            : base(chatId, MessageType.Image)
        {
            TkDebug.AssertArgumentNullOrEmpty(mediaId, nameof(mediaId), null);

            MediaId = mediaId;
        }

        [TagElement(LocalName = "image", Order = 40)]
        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]//媒体文件Id
        public string MediaId { get; private set; }

        //表示是否是保密消息，0表示否，1表示是，默认0
        [SimpleElement(NamingRule = NamingRule.Camel, Order = 100)]
        [TkTypeConverter(typeof(BoolIntConverter))]
        public bool Safe { get; set; }
    }
}