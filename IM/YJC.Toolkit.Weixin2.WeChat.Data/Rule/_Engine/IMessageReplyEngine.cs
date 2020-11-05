using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeChat.Rule
{
    internal interface IMessageReplyEngine
    {
        void Add(RuleAttribute attribute);

        RuleAttribute Match(ReceiveMessage message);
    }
}