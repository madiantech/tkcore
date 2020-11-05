using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    [CacheDependencyStoreConfig(NamespaceType = NamespaceType.Toolkit, Description = "监视登录用户是否改变的缓存依赖",
        Author = "YJC", CreateDate = "2019-10-03", RegName = "UserInfo")]
    internal class UserInfoStoreConfig : IConfigCreator<IDistributeCacheDependency>
    {
        public string UserId { get; set; }

        public IDistributeCacheDependency CreateObject(params object[] args)
        {
            return new UserInfoDependency(this);
        }
    }
}