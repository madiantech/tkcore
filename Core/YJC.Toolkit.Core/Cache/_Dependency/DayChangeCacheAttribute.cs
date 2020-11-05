using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class DayChangeCacheAttribute : CacheDependencyAttribute
    {
        public DayChangeCacheAttribute()
            : this(1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the DayChangeCacheAttribute class.
        /// </summary>
        public DayChangeCacheAttribute(int days)
        {
            Days = days;
        }

        public int Days { get; private set; }

        protected override ICacheDependency CreateCacheDependency()
        {
            return new DayChangeDependency(Days);
        }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture, "隔{0}天失效的缓存特性", Days);
        }
    }
}
