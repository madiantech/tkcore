using System;

namespace YJC.Toolkit.Sys
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class RegClassAttribute : BasePlugInAttribute
    {
        public override string FactoryName
        {
            get
            {
                return RegClassPlugInFactory.REG_NAME;
            }
        }
    }
}
