using System.Collections.Generic;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeCorp.Model.Message
{
    public class NewsCorpMessage : BaseCorpMessage
    {
        public NewsCorpMessage(int agentId)
            : base(agentId, MessageType.News)
        {
        }

        public NewsCorpMessage(string appName)
            : base(appName, MessageType.News)
        {
        }

        [TagElement(LocalName = "news", Order = 50)]
        [ObjectElement(NamingRule = NamingRule.Camel, IsMultiple = true)]
        protected List<NewsArticle> Articles { get; } = new List<NewsArticle>();

        [SimpleElement(NamingRule = NamingRule.UnderLineLower, Order = 120)]
        [TkTypeConverter(typeof(BoolIntConverter))]
        public bool EnableIdTrans { get; set; }

        public void AddArticle(NewsArticle article)
        {
            TkDebug.AssertArgumentNull(article, nameof(article), this);

            Articles.Add(article);
        }
    }
}