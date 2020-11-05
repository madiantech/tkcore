using System.Collections.Generic;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeChat.Rule
{
    internal class NormalReplyEngine : IMessageReplyEngine
    {
        private readonly List<RuleAttribute> fList;

        public NormalReplyEngine()
        {
            fList = new List<RuleAttribute>();
        }

        #region IMessageReplyEngine 成员

        public void Add(RuleAttribute attribute)
        {
            fList.Add(attribute);
        }

        public RuleAttribute Match(ReceiveMessage message)
        {
            foreach (var attr in fList)
            {
                if (attr.Match(message))
                    return attr;
            }

            return null;
        }

        #endregion IMessageReplyEngine 成员
    }
}