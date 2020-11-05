using System.Collections.Generic;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeChat.Rule
{
    internal class EventReplyEngine : IMessageReplyEngine
    {
        private const int EVENT_TYPE_LEN = (int)EventType.LocationSelect + 1;
        private readonly List<RuleAttribute>[] fEventData;

        public EventReplyEngine()
        {
            fEventData = new List<RuleAttribute>[EVENT_TYPE_LEN];
            for (int i = 0; i < EVENT_TYPE_LEN; i++)
                fEventData[i] = new List<RuleAttribute>();
        }

        #region IMessageReplyEngine 成员

        public void Add(RuleAttribute attribute)
        {
            EventRuleAttribute attr = attribute.Convert<EventRuleAttribute>();

            fEventData[(int)attr.EventType].Add(attr);
        }

        public RuleAttribute Match(ReceiveMessage message)
        {
            List<RuleAttribute> list = fEventData[(int)message.Event];
            foreach (var attr in list)
            {
                if (attr.Match(message))
                    return attr;
            }

            return null;
        }

        #endregion IMessageReplyEngine 成员
    }
}