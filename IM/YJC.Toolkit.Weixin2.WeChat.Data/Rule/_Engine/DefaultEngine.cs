using System.Collections.Generic;
using YJC.Toolkit.WeChat.Message;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeChat.Rule
{
    internal class DefaultEngine
    {
        private readonly IRule fDefaultMessage;
        private readonly Dictionary<MessageType, IRule> fDefaultTypeMessage;

        public DefaultEngine(DefaultMessageConfigItem defaultConfig)
        {
            fDefaultMessage = defaultConfig.Global.CreateObject();
            fDefaultTypeMessage = new Dictionary<MessageType, IRule>();
            if (defaultConfig.TypeDefault != null)
            {
                foreach (var item in defaultConfig.TypeDefault)
                    fDefaultTypeMessage[item.MessageType] = item.Message.CreateObject();
            }
        }

        private BaseSendMessage CreateDefaultMessage(ReceiveMessage message)
        {
            IRule defaultReply;
            if (!fDefaultTypeMessage.TryGetValue(message.MsgType, out defaultReply))
                defaultReply = fDefaultMessage;

            return defaultReply.Reply(message);
        }

        public BaseSendMessage Reply(ReceiveMessage message)
        {
            BaseSendMessage result = CreateDefaultMessage(message);
            return result;
        }
    }
}