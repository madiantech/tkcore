using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    [CacheCreator(Author = "YJC", CreateDate = "2019-10-01", Description = "创建Redis缓存")]
    [InstancePlugIn, AlwaysCache]
    internal class RedisCacheCreator : ICacheCreator
    {
        public static readonly ICacheCreator Instance = new RedisCacheCreator();

        private RedisCacheCreator()
        {
        }

        public ICache CreateCache(string cacheName)
        {
            return new RedisDistributedCache();
        }
    }
}