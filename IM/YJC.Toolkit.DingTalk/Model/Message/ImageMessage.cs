using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Message
{
    public class ImageMessage : BaseMessage
    {
        public ImageMessage(string mediaId)
            : base(MessageType.Image)
        {
            TkDebug.AssertArgumentNullOrEmpty(mediaId, "mediaId", null);

            MediaId = mediaId;
        }

        [TagElement(LocalName = "image")]
        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string MediaId { get; private set; }
    }
}