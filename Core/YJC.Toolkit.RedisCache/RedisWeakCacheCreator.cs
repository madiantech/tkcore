using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    [CacheCreator(Author = "YJC", CreateDate = "2019-10-03", Description = "创建本地带有WeakRef的Redis缓存")]
    [InstancePlugIn, AlwaysCache]
    internal class RedisWeakCacheCreator : ICacheCreator
    {
        public static readonly ICacheCreator Instance = new RedisWeakCacheCreator();

        private RedisWeakCacheCreator()
        {
        }

        public ICache CreateCache(string cacheName)
        {
            return new RedisWeakDistributedCache();
        }
    }
}