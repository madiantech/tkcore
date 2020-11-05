using Microsoft.Extensions.Caching.Redis;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    internal static class RedisUtil
    {
        public static RedisCache Cache { get; private set; }

        internal static void CreateCache()
        {
            RedisConfig config = ConfigUtil.ReadConfig(RedisConfig.REG_NAME).Convert<RedisConfig>();
            Cache = new RedisCache(new RedisCacheOptions
            {
                Configuration = config.Configuration,
                InstanceName = config.InstanceName
            });
        }
    }
}