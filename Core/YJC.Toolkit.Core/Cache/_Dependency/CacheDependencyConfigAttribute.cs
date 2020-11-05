using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class CacheDependencyConfigAttribute : BaseObjectElementAttribute
    {
        public override string FactoryName
        {
            get
            {
                return CacheDependencyConfigFactory.REG_NAME;
            }
        }
    }
}
