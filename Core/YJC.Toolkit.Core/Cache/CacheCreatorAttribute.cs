using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class CacheCreatorAttribute : BasePlugInAttribute
    {
        public CacheCreatorAttribute()
        {
            Suffix = "CacheCreator";
        }

        public override string FactoryName
        {
            get
            {
                return CacheCreatorPlugInFactory.REG_NAME;
            }
        }
    }
}
