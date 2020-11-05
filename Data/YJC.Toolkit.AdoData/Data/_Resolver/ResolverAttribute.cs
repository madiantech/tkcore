using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ResolverAttribute : BasePlugInAttribute
    {
        public ResolverAttribute()
        {
            Suffix = "Resolver";
        }

        public override string FactoryName
        {
            get
            {
                return ResolverPlugInFactory.REG_NAME;
            }
        }
    }
}
