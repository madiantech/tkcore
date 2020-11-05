using System;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeChat.Rule
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class LocationEventAttribute : EventRuleAttribute
    {
        public LocationEventAttribute()
            : base(EventType.Location)
        {
        }

        public override bool Match(ReceiveMessage message)
        {
            return true;
        }
    }
}