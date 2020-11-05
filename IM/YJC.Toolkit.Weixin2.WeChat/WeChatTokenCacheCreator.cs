using YJC.Toolkit.Cache;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeChat
{
    [CacheItemCreator(Author = "YJC", CreateDate = "2018-07-04",
        Description = "微信公众号AccessToken缓存器")]
    [InstancePlugIn, AlwaysCache]
    internal class WeChatTokenCacheCreator : BaseCacheItemCreator
    {
        internal static BaseCacheItemCreator Instance = new WeChatTokenCacheCreator();

        private WeChatTokenCacheCreator()
        {
        }

        public override object Create(string key, params object[] args)
        {
            string tenantId = key;
            if (tenantId == IMUtil.NULL_KEY)
                tenantId = null;

            AccessToken token = WeChatConfiguration.ReadToken(tenantId);
            if (token == null)
                return WeChatUtil.CreateToken(tenantId);
            return token;
        }
    }
}