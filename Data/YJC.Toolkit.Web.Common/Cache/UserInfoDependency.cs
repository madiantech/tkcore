using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    internal sealed class UserInfoDependency : ICacheDependency, IDistributeCacheDependency
    {
        private readonly string fUserId;

        public UserInfoDependency()
        {
            fUserId = BaseGlobalVariable.Current.UserInfo.UserId.ConvertToString();
        }

        public UserInfoDependency(UserInfoStoreConfig config)
        {
            fUserId = config.UserId;
        }

        public bool HasChanged
        {
            get
            {
                var info = BaseGlobalVariable.Current.UserInfo;
                if (info == null) // 工作线程中，不检查这个项目
                    return false;
                var userId = info.UserId.ConvertToString();
                return userId != fUserId;
            }
        }

        public object CreateStoredObject()
        {
            return new UserInfoStoreConfig
            {
                UserId = fUserId
            };
        }
    }
}