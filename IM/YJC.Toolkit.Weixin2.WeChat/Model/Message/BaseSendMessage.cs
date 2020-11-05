using System;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeChat.Model.Message
{
    public abstract class BaseSendMessage : BaseWeChatMessage
    {
        private BaseSendMessage(MessageType type)
        {
            MsgType = type;
            CreateTime = DateTime.Now;
        }

        protected BaseSendMessage(string toUser, string fromUser, MessageType type)
            : this(type)
        {
            TkDebug.AssertArgumentNullOrEmpty(toUser, "toUser", null);
            TkDebug.AssertArgumentNullOrEmpty(fromUser, "fromUser", null);

            ToUserName = toUser;
            FromUserName = fromUser;
        }

        protected BaseSendMessage(ReceiveMessage receive, MessageType type)
            : this(type)
        {
            TkDebug.AssertArgumentNull(receive, "receive", null);

            ToUserName = receive.FromUserName;
            FromUserName = receive.ToUserName;
        }
    }
}