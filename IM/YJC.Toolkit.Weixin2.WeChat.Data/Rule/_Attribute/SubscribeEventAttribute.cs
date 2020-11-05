using System;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeChat.Rule
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class SubscribeEventAttribute : EventRuleAttribute
    {
        public SubscribeEventAttribute()
            : base(EventType.Subscribe)
        {
        }

        public override bool Match(ReceiveMessage message)
        {
            //return string.IsNullOrEmpty(message.EventKey);
            return true;
        }
    }
}