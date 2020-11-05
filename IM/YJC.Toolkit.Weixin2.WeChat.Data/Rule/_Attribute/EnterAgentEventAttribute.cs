using System;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeChat.Rule
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class EnterAgentEventAttribute : EventRuleAttribute
    {
        public EnterAgentEventAttribute()
            : base(EventType.EnterAgent)
        {
        }

        public override bool Match(ReceiveMessage message)
        {
            return true;
        }
    }
}