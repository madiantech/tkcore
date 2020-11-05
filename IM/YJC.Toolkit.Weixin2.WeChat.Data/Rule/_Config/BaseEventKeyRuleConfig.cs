using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeChat.Rule
{
    internal abstract class BaseEventKeyRuleConfig : IConfigCreator<RuleAttribute>
    {
        [SimpleAttribute]
        public string EventKey { get; protected set; }

        #region IConfigCreator<RuleAttribute> 成员

        public abstract RuleAttribute CreateObject(params object[] args);

        #endregion IConfigCreator<RuleAttribute> 成员
    }
}