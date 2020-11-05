using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeChat.Rule
{
    [MatchRuleConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-03-04", Description = "匹配自定义菜单事件")]
    internal class EventClickRuleConfig : BaseEventKeyRuleConfig
    {
        public override RuleAttribute CreateObject(params object[] args)
        {
            return new ClickEventAttribute(EventKey);
        }
    }
}