using System.Collections.Generic;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeCorp.Model.Message
{
    public class MpNewsChatMessage : BaseChatMessage
    {
        public MpNewsChatMessage(string chatId)
            : base(chatId, MessageType.MpNews)
        {
        }

        [TagElement(LocalName = "mpnews", Order = 40)]
        [ObjectElement(NamingRule = NamingRule.Camel, IsMultiple = true)]
        protected List<MpNewsArticle> Articles { get; } = new List<MpNewsArticle>();

        //表示是否是保密消息，0表示否，1表示是，默认0
        [SimpleElement(NamingRule = NamingRule.Camel, Order = 100)]
        [TkTypeConverter(typeof(BoolIntConverter))]
        public bool Safe { get; set; }

        public void AddArticle(MpNewsArticle article)
        {
            TkDebug.AssertArgumentNull(article, nameof(article), this);

            Articles.Add(article);
        }
    }
}