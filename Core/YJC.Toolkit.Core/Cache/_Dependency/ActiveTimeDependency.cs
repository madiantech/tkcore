using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    internal sealed class ActiveTimeDependency : ICacheDependency,
        ICacheDependencyTime, IDistributeCacheDependency
    {
        private DateTime fCurrent;
        private readonly TimeSpan fSpan;

        public ActiveTimeDependency(TimeSpan span)
        {
            fCurrent = DateTime.Now;
            fSpan = span;
        }

        public ActiveTimeDependency(ActiveTimeStoreConfig config)
        {
            fCurrent = config.Current;
            fSpan = config.Span;
        }

        #region ICacheDependency 成员

        bool ICacheDependency.HasChanged
        {
            get
            {
                DateTime old = fCurrent + fSpan;
                fCurrent = DateTime.Now;
                return fCurrent >= old;
            }
        }

        public DateTime AbsoluteExpiration => fCurrent + fSpan;

        #endregion ICacheDependency 成员

        public override string ToString()
        {
            return $"每次活动后{fSpan}时间内有效缓存依赖";
        }

        public object CreateStoredObject()
        {
            return new ActiveTimeStoreConfig
            {
                Current = fCurrent,
                Span = fSpan
            };
        }
    }
}