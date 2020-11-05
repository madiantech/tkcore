using System;
using System.Text.RegularExpressions;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeChat.Rule
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class TextRuleAttribute : RuleAttribute
    {
        public TextRuleAttribute(TextMatchType matchType, string text)
            : base(MessageType.Text)
        {
            MatchType = matchType;
            Text = text;
        }

        public TextMatchType MatchType { get; private set; }

        public string Text { get; private set; }

        public override bool Match(ReceiveMessage message)
        {
            return Match(MatchType, Text, message.Content);
        }

        internal static bool Match(TextMatchType matchType, string text, string messageText)
        {
            if (matchType == TextMatchType.All)
                return true;
            if (messageText == null)
                return false;
            switch (matchType)
            {
                case TextMatchType.Exactly:
                    return messageText == text;

                case TextMatchType.StartWith:
                    return messageText.StartsWith(text, StringComparison.Ordinal);

                case TextMatchType.Regex:
                    return Regex.IsMatch(messageText, text,
                        RegexOptions.IgnoreCase | RegexOptions.Singleline);

                default:
                    return false;
            }
        }
    }
}