using YJC.Toolkit.Sys;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeCorp.Model.Message
{
    public class VoiceChatMessage : BaseChatMessage
    {
        public VoiceChatMessage(string chatId, string mediaId)
            : base(chatId, MessageType.Voice)
        {
            TkDebug.AssertArgumentNullOrEmpty(mediaId, nameof(mediaId), null);

            MediaId = mediaId;
        }

        [TagElement(LocalName = "voice", Order = 40)]
        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]//语音文件Id
        public string MediaId { get; private set; }
    }
}