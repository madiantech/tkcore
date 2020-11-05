using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    [CacheInstance]
    [CacheDependencyConfig(NamespaceType = NamespaceType.Toolkit, Description = "永远无效的缓存依赖",
        Author = "YJC", CreateDate = "2013-09-28")]
    [CacheDependencyStoreConfig(NamespaceType = NamespaceType.Toolkit, Description = "永远无效的缓存依赖",
        Author = "YJC", CreateDate = "2019-09-18")]
    internal sealed class NoCacheConfig : IConfigCreator<ICacheDependency>,
        IConfigCreator<IDistributeCacheDependency>
    {
        #region IConfigCreator<ICacheDependency> 成员

        ICacheDependency IConfigCreator<ICacheDependency>.CreateObject(params object[] args)
        {
            return NoDependency.Dependency;
        }

        #endregion IConfigCreator<ICacheDependency> 成员

        public IDistributeCacheDependency CreateObject(params object[] args)
        {
            return NoDependency.InternalDependency;
        }
    }
}