using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public abstract class CacheDependencyAttribute : Attribute, IConfigCreator<ICacheDependency>
    {
        protected CacheDependencyAttribute()
        {
        }

        protected abstract ICacheDependency CreateCacheDependency();

        public ICacheDependency CreateObject(params object[] args)
        {
            return CreateCacheDependency();
        }
    }
}
