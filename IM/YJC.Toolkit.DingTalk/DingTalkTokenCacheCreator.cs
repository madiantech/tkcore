using YJC.Toolkit.Cache;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk
{
    [CacheItemCreator(Author = "YJC", CreateDate = "2016-09-26",
        Description = "钉钉AccessToken缓存器")]
    [InstancePlugIn, AlwaysCache]
    internal class DingTalkTokenCacheCreator : BaseCacheItemCreator
    {
        internal static BaseCacheItemCreator Instance = new DingTalkTokenCacheCreator();

        private DingTalkTokenCacheCreator()
        {
        }

        public override object Create(string key, params object[] args)
        {
            AppCacheKey appKey = AppCacheKey.ReadFromString(key);

            AccessToken token = DingTalkConfiguration.ReadToken(appKey.TenantId, appKey.AppName);
            if (token == null)
                return DingTalkUtil.CreateToken(appKey.TenantId, appKey.AppName);
            return token;
        }
    }
}