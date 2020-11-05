using System;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeChat.Rule
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class LinkRuleAttribute : RuleAttribute
    {
        public LinkRuleAttribute()
            : base(MessageType.Link)
        {
        }

        public override bool Match(ReceiveMessage message)
        {
            return true;
        }
    }
}