using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Weixin.Message
{
    public abstract class BaseWeChatMessage : BaseMessage
    {
        protected BaseWeChatMessage()
        {
        }

        [SimpleElement(Order = 10, UseCData = true)]
        public string ToUserName { get; protected set; }

        [SimpleElement(Order = 20, UseCData = true)]
        public string FromUserName { get; protected set; }

        [SimpleElement(Order = 30)]
        [TkTypeConverter(typeof(IMDateTimeConverter))]
        public DateTime CreateTime { get; protected set; }
    }
}