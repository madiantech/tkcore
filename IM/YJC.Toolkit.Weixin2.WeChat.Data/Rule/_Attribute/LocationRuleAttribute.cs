using System;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeChat.Rule
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class LocationRuleAttribute : RuleAttribute
    {
        public LocationRuleAttribute()
            : base(MessageType.Location)
        {
        }

        public override bool Match(ReceiveMessage message)
        {
            return true;
        }
    }
}