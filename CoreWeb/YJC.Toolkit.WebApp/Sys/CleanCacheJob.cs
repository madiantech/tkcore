using System;
using YJC.Toolkit.Cache;

namespace YJC.Toolkit.Sys
{
    [TimingJob(Author = "YJC", CreateDate = "2019-11-06", Description = DESCRIPTION)]
    internal class CleanCacheJob : ITimingJob
    {
        private const string DESCRIPTION = "清理过期缓存";
        private static readonly TimeSpan INTERVAL = TimeSpan.FromMinutes(5);

        public TimeSpan Interval { get => INTERVAL; }

        public void Process()
        {
            CacheManager.CleanCache();
        }

        public override string ToString() => DESCRIPTION;
    }
}