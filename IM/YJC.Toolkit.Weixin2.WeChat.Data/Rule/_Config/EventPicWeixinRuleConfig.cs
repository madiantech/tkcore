using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeChat.Rule
{
    [MatchRuleConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-12-28", Description = "匹配弹出微信相册发图器的事件")]
    internal class EventPicWeixinRuleConfig : BaseEventKeyRuleConfig
    {
        public override RuleAttribute CreateObject(params object[] args)
        {
            return new PicWeixinEventAttribute(EventKey);
        }
    }
}
