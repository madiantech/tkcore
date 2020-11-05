using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    [Config(RegName = REG_NAME, Author = "YJC", CreateDate = "2019-09-30", Description = "Redis的配置")]
    public class RedisConfig
    {
        public const string REG_NAME = "RedisCache";

        [SimpleElement(Required = true)]
        public string Configuration { get; private set; }

        [SimpleElement]
        public string InstanceName { get; private set; }
    }
}