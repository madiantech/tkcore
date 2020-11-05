using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeChat.Rule
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ReplyMessageConfigAttribute : BaseObjectElementAttribute
    {
        public override string FactoryName
        {
            get
            {
                return ReplyMessageConfigFactory.REG_NAME;
            }
        }
    }
}