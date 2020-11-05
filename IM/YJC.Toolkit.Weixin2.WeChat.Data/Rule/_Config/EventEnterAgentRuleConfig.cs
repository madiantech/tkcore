using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeChat.Rule
{
    [MatchRuleConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-12-26", Description = "匹配企业号用户进入应用的事件")]
    internal class EventEnterAgentRuleConfig : IConfigCreator<RuleAttribute>
    {
        #region IConfigCreator<RuleAttribute> 成员

        public RuleAttribute CreateObject(params object[] args)
        {
            return new EnterAgentEventAttribute();
        }

        #endregion IConfigCreator<RuleAttribute> 成员
    }
}