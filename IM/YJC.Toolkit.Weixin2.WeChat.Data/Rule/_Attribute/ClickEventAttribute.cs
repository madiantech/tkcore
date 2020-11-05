using System;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeChat.Rule
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ClickEventAttribute : BaseClickEventAttribute
    {
        public ClickEventAttribute(string eventKey)
            : base(eventKey, EventType.Click)
        {
        }
    }
}