using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeChat.Rule
{
    public abstract class BaseClickEventAttribute : EventRuleAttribute
    {
        protected BaseClickEventAttribute(string eventKey, EventType type)
            : base(type)
        {
            EventKey = eventKey;
        }

        public string EventKey { get; private set; }

        public override bool Match(ReceiveMessage message)
        {
            return message.EventKey == EventKey;
        }
    }
}