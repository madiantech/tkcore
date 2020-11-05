using YJC.Toolkit.Sys;
using YJC.Toolkit.WeChat.Model.Message;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeChat.Rule
{
    internal class MessageEngine
    {
        private const int MESSAGE_TYPE_COUNT = (int)MessageType.Event + 1;
        private readonly IMessageReplyEngine[] fEngineList;

        public MessageEngine()
        {
            fEngineList = new IMessageReplyEngine[MESSAGE_TYPE_COUNT];

            for (int i = 0; i < MESSAGE_TYPE_COUNT; i++)
            {
                MessageType type = (MessageType)i;
                switch (type)
                {
                    case MessageType.Event:
                        fEngineList[i] = new EventReplyEngine();
                        break;

                    case MessageType.Text:
                    case MessageType.Voice:
                    case MessageType.Image:
                    case MessageType.Video:
                    case MessageType.Location:
                    case MessageType.Link:
                        fEngineList[i] = new NormalReplyEngine();
                        break;

                    default:
                        fEngineList[i] = EmptyReplyEngine.Engine;
                        break;
                }
            }
        }

        public void Add(RuleAttribute attr)
        {
            IMessageReplyEngine subEngine = fEngineList[(int)attr.MsgType];
            subEngine.Add(attr);
        }

        public bool Reply(ReceiveMessage message, out BaseSendMessage replyMessage)
        {
            IMessageReplyEngine subEngine = fEngineList[(int)message.MsgType];
            RuleAttribute attr = subEngine.Match(message);
            if (attr != null)
            {
                if (attr == RuleAttribute.Empty)
                    replyMessage = null;
                else
                {
                    IRule reply = PlugInFactoryManager.CreateInstance<IRule>(
                        RulePlugInFactory.REG_NAME, attr.RegName).Convert<IRule>();
                    replyMessage = reply.Reply(message);
                }
                return true;
            }
            else
            {
                replyMessage = null;
                return false;
            }
        }
    }
}