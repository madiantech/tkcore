using YJC.Toolkit.Cache;

namespace YJC.Toolkit.Sys
{
    [CacheItemCreator(Author = "YJC", CreateDate = "2019-10-04",
        Description = "SessionGlobal的缓存对象创建器")]
    [InstancePlugIn, AlwaysCache]
    internal class SessionGlobalCacheCreator : BaseCacheItemCreator
    {
        public static BaseCacheItemCreator Instance = new SessionGlobalCacheCreator();

        private SessionGlobalCacheCreator()
        {
        }

        public override object Create(string key, params object[] args)
        {
            return SessionGlobal.CreateSessionGlobal(key);
        }
    }
}