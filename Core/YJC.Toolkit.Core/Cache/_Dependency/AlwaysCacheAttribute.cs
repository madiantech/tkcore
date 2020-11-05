using System;

namespace YJC.Toolkit.Cache
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class AlwaysCacheAttribute : CacheDependencyAttribute
    {
        protected override ICacheDependency CreateCacheDependency()
        {
            return AlwaysDependency.Dependency;
        }

        public override string ToString()
        {
            return "永远存在的缓存特性";
        }
    }
}
