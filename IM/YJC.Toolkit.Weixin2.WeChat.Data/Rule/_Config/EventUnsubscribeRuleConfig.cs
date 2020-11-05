using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeChat.Rule
{
    [MatchRuleConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-03-06", Description = "匹配取消事件")]
    internal class EventUnsubscribeRuleConfig : IConfigCreator<RuleAttribute>
    {
        #region IConfigCreator<RuleAttribute> 成员

        public RuleAttribute CreateObject(params object[] args)
        {
            return new UnsubscribeEventAttribute();
        }

        #endregion
    }
}
