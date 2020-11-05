using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeChat.Rule
{
    [MatchRuleConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-12-28", Description = "匹配弹出地理位置选择器的事件")]
    internal class EventLocationSelectRuleConfig : BaseEventKeyRuleConfig
    {
        public override RuleAttribute CreateObject(params object[] args)
        {
            return new LocationSelectEventAttribute(EventKey);
        }
    }
}
