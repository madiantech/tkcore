using System;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeChat.Rule
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class VoiceRuleAttribute : RuleAttribute
    {
        public VoiceRuleAttribute(TextMatchType matchType, string text)
            : base(MessageType.Voice)
        {
            MatchType = matchType;
            Text = text;
        }

        public TextMatchType MatchType { get; private set; }

        public string Text { get; private set; }

        public override bool Match(ReceiveMessage message)
        {
            if (string.IsNullOrEmpty(Text))
                return true;
            return TextRuleAttribute.Match(MatchType, Text, message.Recognition);
        }
    }
}