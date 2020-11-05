using YJC.Toolkit.Sys;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeCorp.Model.Message
{
    public class FileChatMessage : BaseChatMessage
    {
        public FileChatMessage(string chatId, string mediaId)
            : base(chatId, MessageType.File)
        {
            TkDebug.AssertArgumentNullOrEmpty(mediaId, nameof(mediaId), null);

            MediaId = mediaId;
        }

        [TagElement(LocalName = "file", Order = 40)]
        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]//文件
        public string MediaId { get; private set; }

        //表示是否是保密消息，0表示否，1表示是，默认0
        [SimpleElement(NamingRule = NamingRule.Camel, Order = 100)]
        [TkTypeConverter(typeof(BoolIntConverter))]
        public bool Safe { get; set; }
    }
}