using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeChat.Rule
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class MatchRuleConfigAttribute : BaseObjectElementAttribute
    {
        public override string FactoryName
        {
            get
            {
                return MatchRuleConfigFactory.REG_NAME;
            }
        }
    }
}