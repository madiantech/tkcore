using System;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeChat.Rule
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class VideoRuleAttribute : RuleAttribute
    {
        public VideoRuleAttribute()
            : base(MessageType.Video)
        {
        }

        public override bool Match(ReceiveMessage message)
        {
            return true;
        }
    }
}