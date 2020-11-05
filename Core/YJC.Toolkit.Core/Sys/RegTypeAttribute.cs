using System;

namespace YJC.Toolkit.Sys
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class RegTypeAttribute : BasePlugInAttribute
    {
        public override string FactoryName
        {
            get
            {
                return RegTypeFactory.REG_NAME;
            }
        }
    }
}
