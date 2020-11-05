using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeChat.Rule
{
    [MatchRuleConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-12-28", Description = "匹配扫码推事件且弹出“消息接收中”提示框的事件")]
    internal class EventScanCodeWaitMsgRuleConfig : BaseEventKeyRuleConfig
    {
        public override RuleAttribute CreateObject(params object[] args)
        {
            return new ScanCodeWaitmsgEventAttribute(EventKey);
        }
    }
}
