using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    [CacheDependencyConfig(NamespaceType = NamespaceType.Toolkit, Description = "永久有效的缓存依赖",
        Author = "YJC", CreateDate = "2013-09-28")]
    [CacheDependencyStoreConfig(NamespaceType = NamespaceType.Toolkit, Description = "永久有效的缓存依赖",
        Author = "YJC", CreateDate = "2019-09-18")]
    [CacheInstance]
    internal class AlwaysConfig : IConfigCreator<ICacheDependency>,
        IConfigCreator<IDistributeCacheDependency>
    {
        public IDistributeCacheDependency CreateObject(params object[] args)
        {
            return AlwaysDependency.InternalDependency;
        }

        #region IConfigCreator<ICacheDependency> 成员

        ICacheDependency IConfigCreator<ICacheDependency>.CreateObject(params object[] args)
        {
            return AlwaysDependency.Dependency;
        }

        #endregion IConfigCreator<ICacheDependency> 成员
    }
}