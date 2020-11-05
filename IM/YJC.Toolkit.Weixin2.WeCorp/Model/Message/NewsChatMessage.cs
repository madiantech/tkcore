using System.Collections.Generic;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeCorp.Model.Message
{
    public class NewsChatMessage : BaseChatMessage
    {
        public NewsChatMessage(string chatId)
            : base(chatId, MessageType.News)
        {
        }

        [TagElement(LocalName = "news", Order = 40)]
        [ObjectElement(NamingRule = NamingRule.Camel, IsMultiple = true)]
        protected List<NewsArticle> Articles { get; } = new List<NewsArticle>();

        //表示是否是保密消息，0表示否，1表示是，默认0
        [SimpleElement(NamingRule = NamingRule.Camel, Order = 100)]
        [TkTypeConverter(typeof(BoolIntConverter))]
        public bool Safe { get; set; }

        public void AddArticle(NewsArticle article)
        {
            TkDebug.AssertArgumentNull(article, nameof(article), this);

            Articles.Add(article);
        }
    }
}