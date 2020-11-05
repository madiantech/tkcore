using System;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeChat.Rule
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ScanEventAttribute : EventRuleAttribute
    {
        public ScanEventAttribute()
            : base(EventType.Scan)
        {
        }

        public string EventKey { get; set; }

        public override bool Match(ReceiveMessage message)
        {
            bool result = true;
            if (!string.IsNullOrEmpty(EventKey))
                result &= message.EventKey == EventKey;
            return result;
        }
    }
}