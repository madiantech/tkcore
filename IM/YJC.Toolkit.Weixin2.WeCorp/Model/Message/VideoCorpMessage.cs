using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeCorp.Model.Message
{
    public class VideoCorpMessage : BaseCorpMessage
    {
        public VideoCorpMessage(int agentId, VideoMedia video)
            : base(agentId, MessageType.Video)
        {
            TkDebug.AssertArgumentNull(video, nameof(video), null);

            Video = video;
        }

        public VideoCorpMessage(string appName, VideoMedia video)
            : base(appName, MessageType.Video)
        {
            TkDebug.AssertArgumentNull(video, nameof(video), null);

            Video = video;
        }

        [ObjectElement(NamingRule = NamingRule.Camel, Order = 50)]
        public VideoMedia Video { get; private set; }

        [SimpleElement(NamingRule = NamingRule.Camel, Order = 100)]//是否保密发送//UseSourceType = true,
        [TkTypeConverter(typeof(BoolIntConverter))]
        public bool Safe { get; set; }
    }
}