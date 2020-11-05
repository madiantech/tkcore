using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeChat.Rule
{
    public abstract class EventRuleAttribute : RuleAttribute
    {
        protected EventRuleAttribute(EventType eventType)
            : base(MessageType.Event)
        {
            EventType = eventType;
        }

        public EventType EventType { get; private set; }
    }
}