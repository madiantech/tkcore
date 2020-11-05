using System.Collections.Generic;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeCorp.Model.Message
{
    public class MpNewsCorpMessage : BaseCorpMessage
    {
        public MpNewsCorpMessage(int agentId)
            : base(agentId, MessageType.MpNews)
        {
        }

        public MpNewsCorpMessage(string appName)
            : base(appName, MessageType.MpNews)
        {
        }

        [TagElement(LocalName = "mpnews", Order = 50)]
        [ObjectElement(NamingRule = NamingRule.Camel, IsMultiple = true)]
        protected List<MpNewsArticle> Articles { get; } = new List<MpNewsArticle>();

        [SimpleElement(NamingRule = NamingRule.Camel, Order = 100)]//是否保密发送//UseSourceType = true,
        [TkTypeConverter(typeof(BoolIntConverter))]
        public bool Safe { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower, Order = 120)]
        [TkTypeConverter(typeof(BoolIntConverter))]
        public bool EnableIdTrans { get; set; }

        public void AddArticle(MpNewsArticle article)
        {
            TkDebug.AssertArgumentNull(article, nameof(article), this);

            Articles.Add(article);
        }
    }
}