using System;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeChat.Rule
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class UnsubscribeEventAttribute : EventRuleAttribute
    {
        public UnsubscribeEventAttribute()
            : base(EventType.Unsubscribe)
        {
        }

        public override bool Match(ReceiveMessage message)
        {
            return true;
        }
    }
}