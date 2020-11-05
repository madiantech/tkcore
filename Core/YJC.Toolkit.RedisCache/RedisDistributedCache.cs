namespace YJC.Toolkit.Cache
{
    public class RedisDistributedCache : DistributedCache
    {
        public RedisDistributedCache() : base(RedisUtil.Cache)
        {
        }
    }
}