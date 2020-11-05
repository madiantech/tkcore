using System;

namespace YJC.Toolkit.Sys
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ExceptionHandlerConfigAttribute : BaseObjectElementAttribute
    {
        public override string FactoryName
        {
            get
            {
                return ExceptionHanlderConfigFactory.REG_NAME;
            }
        }
    }
}
