using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeCorp.Model.Message
{
    public class TextCardChatMessage : BaseChatMessage
    {
        public TextCardChatMessage(string chatId, TextCard textCard)
            : base(chatId, MessageType.TextCard)
        {
            TkDebug.AssertArgumentNull(textCard, nameof(textCard), null);

            TextCard = textCard;
        }

        [ObjectElement(NamingRule = NamingRule.Lower, Order = 40)]
        public TextCard TextCard { get; private set; }

        //表示是否是保密消息，0表示否，1表示是，默认0
        [SimpleElement(NamingRule = NamingRule.Camel, Order = 100)]
        [TkTypeConverter(typeof(BoolIntConverter))]
        public bool Safe { get; set; }
    }
}