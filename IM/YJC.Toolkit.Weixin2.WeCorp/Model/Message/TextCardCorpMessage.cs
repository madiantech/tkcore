using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeCorp.Model.Message
{
    public class TextCardCorpMessage : BaseCorpMessage
    {
        public TextCardCorpMessage(int agentId, TextCard textCard)
            : base(agentId, MessageType.TextCard)
        {
            TkDebug.AssertArgumentNull(textCard, nameof(textCard), null);

            TextCard = textCard;
        }

        public TextCardCorpMessage(string appName, TextCard textCard)
            : base(appName, MessageType.TextCard)
        {
            TkDebug.AssertArgumentNull(textCard, nameof(textCard), null);

            TextCard = textCard;
        }

        //[TagElement(LocalName = "news", Order = 50)]
        [ObjectElement(NamingRule = NamingRule.Lower, Order = 50)]
        public TextCard TextCard { get; private set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower, Order = 120)]
        [TkTypeConverter(typeof(BoolIntConverter))]
        public bool EnableIdTrans { get; set; }
    }
}