using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeChat.Rule
{
    [MatchRuleConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
       CreateDate = "2014-03-04", Description = "匹配关注事件")]
    internal class EventSubscribeRuleConfig : IConfigCreator<RuleAttribute>
    {
        #region IConfigCreator<RuleAttribute> 成员

        public RuleAttribute CreateObject(params object[] args)
        {
            return new SubscribeEventAttribute();
        }

        #endregion
    }
}
