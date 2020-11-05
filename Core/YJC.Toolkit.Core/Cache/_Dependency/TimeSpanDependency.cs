using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    internal sealed class TimeSpanDependency : ICacheDependency, ICacheDependencyTime, IDistributeCacheDependency
    {
        private readonly DateTime fExpireTime;

        /// <summary>
        /// Initializes a new instance of the TimeSpanDependency class.
        /// </summary>
        public TimeSpanDependency(TimeSpan span)
        {
            fExpireTime = DateTime.Now + span;
        }

        public TimeSpanDependency(TimeSpanStoreConfig config)
        {
            fExpireTime = config.ExpireTime;
        }

        public DateTime AbsoluteExpiration => fExpireTime;

        #region ICacheDependency 成员

        bool ICacheDependency.HasChanged
        {
            get
            {
                return DateTime.Now > fExpireTime;
            }
        }

        #endregion ICacheDependency 成员

        public object CreateStoredObject()
        {
            return new TimeSpanStoreConfig
            {
                ExpireTime = fExpireTime
            };
        }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture,
                "在{0}时间内有效的缓存依赖", fExpireTime);
        }
    }
}