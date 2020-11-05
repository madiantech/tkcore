using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeCorp.Model.Message
{
    public class VoiceCorpMessage : BaseCorpMessage
    {
        public VoiceCorpMessage(int agentId, string mediaId)
            : base(agentId, MessageType.Voice)
        {
            TkDebug.AssertArgumentNullOrEmpty(mediaId, nameof(mediaId), null);

            MediaId = mediaId;
        }

        public VoiceCorpMessage(string appName, string mediaId)
            : base(appName, MessageType.Voice)
        {
            TkDebug.AssertArgumentNullOrEmpty(mediaId, nameof(mediaId), null);

            MediaId = mediaId;
        }

        [TagElement(LocalName = "voice", Order = 50)]
        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]//语音文件id
        public string MediaId { get; set; }
    }
}