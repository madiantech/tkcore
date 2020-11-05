using System;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeChat.Rule
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ImageRuleAttribute : RuleAttribute
    {
        public ImageRuleAttribute()
            : base(MessageType.Image)
        {
        }

        public override bool Match(ReceiveMessage message)
        {
            return true;
        }
    }
}