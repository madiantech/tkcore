using System;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeChat.Rule
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class PicWeixinEventAttribute : BaseClickEventAttribute
    {
        public PicWeixinEventAttribute(string eventKey)
            : base(eventKey, EventType.PicWeixin)
        {
        }
    }
}