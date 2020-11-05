using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class TypeSchemeAttribute : BasePlugInAttribute
    {
        public override string FactoryName
        {
            get
            {
                return TypeSchemeTypeFactory.REG_NAME;
            }
        }
    }
}
