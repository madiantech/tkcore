using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeChat.Rule
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ReplyMessageAttribute : BasePlugInAttribute
    {
        public ReplyMessageAttribute()
            : base()
        {
            Suffix = "Rule";
        }

        public override string FactoryName
        {
            get
            {
                return ReplyMessagePlugInFactory.REG_NAME;
            }
        }
    }
}