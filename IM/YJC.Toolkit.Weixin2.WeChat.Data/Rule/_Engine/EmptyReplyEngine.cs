using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeChat.Rule
{
    internal class EmptyReplyEngine : IMessageReplyEngine
    {
        public static readonly IMessageReplyEngine Engine = new EmptyReplyEngine();

        private EmptyReplyEngine()
        {
        }

        #region IMessageReplyEngine 成员

        public void Add(RuleAttribute attribute)
        {
        }

        public RuleAttribute Match(ReceiveMessage message)
        {
            return RuleAttribute.Empty;
        }

        #endregion IMessageReplyEngine 成员
    }
}