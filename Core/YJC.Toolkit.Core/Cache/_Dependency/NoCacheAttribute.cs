using System;

namespace YJC.Toolkit.Cache
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class NoCacheAttribute : CacheDependencyAttribute
    {
        protected override ICacheDependency CreateCacheDependency()
        {
            return NoDependency.Dependency;
        }

        public override string ToString()
        {
            return "永远不会缓存的缓存特性";
        }
    }
}