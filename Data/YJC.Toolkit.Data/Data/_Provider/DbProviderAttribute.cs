using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class DbProviderAttribute : BasePlugInAttribute
    {
        public DbProviderAttribute()
        {
            Suffix = "DbProvider";
        }

        public override string FactoryName
        {
            get
            {
                return DbProviderPlugInFactory.REG_NAME;
            }
        }
    }
}
