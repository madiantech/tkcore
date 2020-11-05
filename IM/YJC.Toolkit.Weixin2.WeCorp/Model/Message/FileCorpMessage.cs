using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeCorp.Model.Message
{
    public class FileCorpMessage : BaseCorpMessage
    {
        public FileCorpMessage(int agentId, string mediaId)
            : base(agentId, MessageType.File)
        {
            TkDebug.AssertArgumentNullOrEmpty(mediaId, nameof(mediaId), null);

            MediaId = mediaId;
        }

        public FileCorpMessage(string appName, string mediaId)
            : base(appName, MessageType.File)
        {
            TkDebug.AssertArgumentNullOrEmpty(mediaId, nameof(mediaId), null);

            MediaId = mediaId;
        }

        [TagElement(LocalName = "file")]
        [SimpleElement(NamingRule = NamingRule.UnderLineLower, Order = 50)]//语音文件id
        public string MediaId { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel, Order = 100)]//是否保密发送//UseSourceType = true,
        [TkTypeConverter(typeof(BoolIntConverter))]
        public bool Safe { get; set; }
    }
}