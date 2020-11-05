namespace YJC.Toolkit.Cache
{
    public class RedisWeakDistributedCache : WeakDistributedCache
    {
        public RedisWeakDistributedCache() : base(RedisUtil.Cache)
        {
        }
    }
}