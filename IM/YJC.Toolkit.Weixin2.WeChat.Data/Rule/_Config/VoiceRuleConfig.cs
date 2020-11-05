using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeChat.Rule
{
    [MatchRuleConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-03-06", Description = "匹配接收的语音消息")]
    internal class VoiceRuleConfig : IConfigCreator<RuleAttribute>
    {
        #region IConfigCreator<RuleAttribute> 成员

        public RuleAttribute CreateObject(params object[] args)
        {
            return new VoiceRuleAttribute(MatchType, Text);
        }

        #endregion

        [SimpleAttribute]
        public TextMatchType MatchType { get; private set; }

        [SimpleAttribute]
        public string Text { get; private set; }
    }
}
