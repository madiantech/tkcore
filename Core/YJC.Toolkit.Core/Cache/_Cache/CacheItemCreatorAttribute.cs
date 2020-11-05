using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class CacheItemCreatorAttribute : BasePlugInAttribute
    {
        public CacheItemCreatorAttribute()
        {
            Suffix = "CacheCreator";
        }

        public bool ForceCache { get; set; }

        public override string FactoryName
        {
            get
            {
                return CacheItemCreatorPlugInFactory.REG_NAME;
            }
        }
    }
}
