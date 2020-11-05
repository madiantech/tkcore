using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeCorp.Model.Message
{
    public class ImageCorpMessage : BaseCorpMessage
    {
        public ImageCorpMessage(int agentId, string mediaId)
            : base(agentId, MessageType.Image)
        {
            TkDebug.AssertArgumentNullOrEmpty(mediaId, nameof(mediaId), null);

            MediaId = mediaId;
        }

        public ImageCorpMessage(string appName, string mediaId)
            : base(appName, MessageType.Image)
        {
            TkDebug.AssertArgumentNullOrEmpty(mediaId, nameof(mediaId), null);

            MediaId = mediaId;
        }

        [SimpleElement(NamingRule = NamingRule.Camel, Order = 100)]//是否保密发送//UseSourceType = true,
        [TkTypeConverter(typeof(BoolIntConverter))]
        public bool Safe { get; set; }

        [TagElement(LocalName = "image")]
        [SimpleElement(NamingRule = NamingRule.UnderLineLower, Order = 50)]//图片媒体文件id
        public string MediaId { get; set; }
    }
}