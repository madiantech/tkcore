using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeCorp.Model.Message
{
    public class TextCorpMessage : BaseCorpMessage
    {
        public TextCorpMessage(int agentId, string content)
            : base(agentId, MessageType.Text)
        {
            TkDebug.AssertArgumentNullOrEmpty(content, nameof(content), null);

            Content = content;
        }

        public TextCorpMessage(string appName, string content)
            : base(appName, MessageType.Text)
        {
            TkDebug.AssertArgumentNullOrEmpty(content, nameof(content), null);

            Content = content;
        }

        [TagElement(LocalName = "text", Order = 50)]
        [SimpleElement(NamingRule = NamingRule.Camel)]//文本内容
        public string Content { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel, Order = 100)]//是否保密发送//UseSourceType = true,
        [TkTypeConverter(typeof(BoolIntConverter))]
        public bool Safe { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower, Order = 120)]
        [TkTypeConverter(typeof(BoolIntConverter))]
        public bool EnableIdTrans { get; set; }
    }
}