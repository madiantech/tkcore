using YJC.Toolkit.Cache;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeCorp
{
    [CacheItemCreator(Author = "YJC", CreateDate = "2016-09-26",
        Description = "微信企业号AccessToken缓存器")]
    [InstancePlugIn, AlwaysCache]
    internal class WeCorpTokenCacheCreator : BaseCacheItemCreator
    {
        internal static BaseCacheItemCreator Instance = new WeCorpTokenCacheCreator();

        private WeCorpTokenCacheCreator()
        {
        }

        public override object Create(string key, params object[] args)
        {
            WeCorpCacheKey corpKey = WeCorpCacheKey.ReadFromString(key);
            AccessToken token = WeCorpConfiguration.ReadToken(corpKey.TenantId, corpKey.AppName);
            if (token == null)
                return WeCorpUtil.CreateToken(corpKey.TenantId, corpKey.AppName);
            return token;
        }
    }
}