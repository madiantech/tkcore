using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;
using YJC.Toolkit.WeChat.Model.Message;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeChat.Rule
{
    [ReplyMessageConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-03-03", Description = "文本消息")]
    internal class TextMessageConfig : IRule, IConfigCreator<IRule>
    {
        [ObjectElement(NamespaceType.Toolkit)]
        public MultiLanguageText Content { get; private set; }

        #region IReplyMessage 成员

        public BaseSendMessage Reply(ReceiveMessage message)
        {
            return new TextSendMessage(message, Content.ToString());
        }

        #endregion IReplyMessage 成员

        #region IConfigCreator<IReplyMessage> 成员

        public IRule CreateObject(params object[] args)
        {
            return this;
        }

        #endregion IConfigCreator<IReplyMessage> 成员
    }
}