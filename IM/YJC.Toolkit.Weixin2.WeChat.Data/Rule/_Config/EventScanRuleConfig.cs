using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeChat.Rule
{
    [MatchRuleConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-03-06", Description = "匹配扫描带参数二维码事件")]
    class EventScanRuleConfig : IConfigCreator<RuleAttribute>
    {
        #region IConfigCreator<RuleAttribute> 成员

        public RuleAttribute CreateObject(params object[] args)
        {
            return new ScanEventAttribute
            {
                EventKey = EventKey
            };
        }

        #endregion

        [SimpleAttribute]
        public string EventKey { get; private set; }
    }
}
