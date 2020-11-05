using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ActiveTimeCacheAttribute : CacheDependencyAttribute
    {
        private readonly bool fSystemConfiged;
        private readonly TimeSpan fTimeSpan;

        public ActiveTimeCacheAttribute()
        {
            fSystemConfiged = true;
        }

        public ActiveTimeCacheAttribute(int hour)
        {
            TkDebug.AssertArgument(hour > 0, "hour", "hour参数不能为负数", null);

            Hour = hour;
            fTimeSpan = new TimeSpan(hour, 0, 0);
        }

        public ActiveTimeCacheAttribute(string span)
        {
            TkDebug.AssertArgumentNullOrEmpty(span, "span", null);

            Span = span;
            try
            {
                fTimeSpan = TimeSpan.Parse(span, ObjectUtil.SysCulture);
            }
            catch
            {
                TkDebug.ThrowToolkitException(
                    string.Format(ObjectUtil.SysCulture,
                    "{0}不是一个有效的TimeSpan格式，请参考MSDN文档", span), null);
            }
        }

        public int Hour { get; private set; }

        public string Span { get; private set; }

        protected override ICacheDependency CreateCacheDependency()
        {
            if (fSystemConfiged)
            {
                TkDebug.ThrowIfNoAppSetting();
                return new ActiveTimeDependency(BaseAppSetting.Current.CacheTime);
            }
            else
                return new ActiveTimeDependency(fTimeSpan);
        }

        public override string ToString()
        {
            return fSystemConfiged ? "每次活动后在系统配置的时间内有效缓存特性" :
                string.Format(ObjectUtil.SysCulture,
                "每次活动后{0}时间内有效缓存特性", fTimeSpan);
        }
    }
}
