using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Message
{
    public abstract class BaseMessage
    {
        private readonly MessageType fMessageType;

        protected BaseMessage(MessageType messageType)
        {
            fMessageType = messageType;
        }

        [SimpleElement(NamingRule = NamingRule.Lower)]
        [TkTypeConverter(typeof(EnumFieldValueTypeConverter), UseObjectType = true)]
        public MessageType MsgType
        {
            get
            {
                return fMessageType;
            }
            set
            {
            }
        }
    }
}