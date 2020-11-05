using System;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeChat.Rule
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ScanCodePushEventAttribute : BaseClickEventAttribute
    {
        public ScanCodePushEventAttribute(string eventKey)
            : base(eventKey, EventType.ScanCodePush)
        {
        }
    }
}