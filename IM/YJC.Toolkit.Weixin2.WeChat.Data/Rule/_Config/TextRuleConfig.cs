using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeChat.Rule
{
    [MatchRuleConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-03-06", Description = "匹配接收的文本消息")]
    internal class TextRuleConfig : IConfigCreator<RuleAttribute>
    {
        #region IConfigCreator<RuleAttribute> 成员

        public RuleAttribute CreateObject(params object[] args)
        {
            return new TextRuleAttribute(MatchType, Text);
        }

        #endregion IConfigCreator<RuleAttribute> 成员

        [SimpleAttribute(Required = true)]
        public TextMatchType MatchType { get; private set; }

        [SimpleAttribute(Required = true)]
        public string Text { get; private set; }
    }
}