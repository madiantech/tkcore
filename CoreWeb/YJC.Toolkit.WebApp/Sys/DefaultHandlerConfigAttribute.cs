using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Sys
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class DefaultHandlerConfigAttribute : BaseObjectElementAttribute
    {
        public override string FactoryName
        {
            get
            {
                return DefaultHandlerConfigFactory.REG_NAME;
            }
        }
    }
}
