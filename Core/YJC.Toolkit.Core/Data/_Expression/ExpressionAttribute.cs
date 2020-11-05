using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ExpressionAttribute : BasePlugInAttribute
    {
        public ExpressionAttribute()
        {
            Suffix = "Expression";
        }

        public bool SqlInject { get; set; }

        public override string FactoryName
        {
            get
            {
                return ExpressionPlugInFactory.REG_NAME;
            }
        }
    }
}
