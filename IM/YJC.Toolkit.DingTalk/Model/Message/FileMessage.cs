using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Message
{
    public class FileMessage : BaseMessage
    {
        public FileMessage(string mediaId)
            : base(MessageType.File)
        {
            TkDebug.AssertArgumentNullOrEmpty(mediaId, "mediaId", null);
            MediaId = mediaId;
        }

        [TagElement(LocalName = "file")]
        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string MediaId { get; private set; }
    }
}