using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    internal sealed class CacheData : IDisposable
    {
        public CacheData(object data, ICacheDependency dependency)
        {
            Data = data;
            Dependency = dependency;
        }

        public object Data { get; private set; }

        public ICacheDependency Dependency { get; private set; }

        #region IDisposable 成员

        public void Dispose()
        {
            Data.DisposeObject();
            Dependency.DisposeObject();
        }

        #endregion
    }
}
