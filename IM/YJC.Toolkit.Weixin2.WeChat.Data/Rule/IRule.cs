using YJC.Toolkit.WeChat.Model.Message;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeChat.Rule
{
    public interface IRule
    {
        BaseSendMessage Reply(ReceiveMessage message);
    }
}