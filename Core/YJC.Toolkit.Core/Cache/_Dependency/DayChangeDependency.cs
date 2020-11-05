using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    internal class DayChangeDependency : ICacheDependency,
        ICacheDependencyTime, IDistributeCacheDependency
    {
        private DateTime fCacheDay;

        public DayChangeDependency()
        {
            fCacheDay = DateTime.Today;
            Days = 1;
        }

        public DayChangeDependency(int days)
            : this()
        {
            Days = days;
        }

        public DayChangeDependency(DayChangeStoreConfig config)
        {
            fCacheDay = config.CacheDay;
            Days = config.Days;
        }

        public int Days { get; private set; }

        public DateTime AbsoluteExpiration => fCacheDay.AddDays(Days);

        #region ICacheDependency 成员

        bool ICacheDependency.HasChanged
        {
            get
            {
                DateTime today = DateTime.Today;
                if (fCacheDay.AddDays(Days - 1) >= today)
                    return false;
                else
                {
                    //fCacheDay = today;
                    return true;
                }
            }
        }

        #endregion ICacheDependency 成员

        public object CreateStoredObject()
        {
            return new DayChangeStoreConfig
            {
                CacheDay = fCacheDay,
                Days = Days
            };
        }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture, "隔{0}天失效的缓存依赖", Days); ;
        }
    }
}