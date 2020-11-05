using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    [CacheDependencyConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        Description = "监视登录用户是否改变的缓存依赖", CreateDate = "2019-10-03")]
    internal class UserInfoConfig : IConfigCreator<ICacheDependency>
    {
        public ICacheDependency CreateObject(params object[] args)
        {
            return new UserInfoDependency();
        }
    }
}