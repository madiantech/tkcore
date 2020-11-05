using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class SqlProviderAttribute : BasePlugInAttribute
    {
        public SqlProviderAttribute()
        {
            Suffix = "SqlProvider";
        }

        public override string FactoryName
        {
            get
            {
                return SqlProviderPlugInFactory.REG_NAME;
            }
        }
    }
}
