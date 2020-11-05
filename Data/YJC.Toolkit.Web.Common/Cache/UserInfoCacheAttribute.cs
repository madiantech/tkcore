namespace YJC.Toolkit.Cache
{
    public sealed class UserInfoCacheAttribute : CacheDependencyAttribute
    {
        protected override ICacheDependency CreateCacheDependency()
        {
            return new UserInfoDependency();
        }

        public override string ToString()
        {
            return "监视登录用户是否改变的缓存依赖";
        }
    }
}