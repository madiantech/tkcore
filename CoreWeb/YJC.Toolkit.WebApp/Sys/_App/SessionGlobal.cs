using System;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Right;
using YJC.Toolkit.Threading;

namespace YJC.Toolkit.Sys
{
    [UserInfoCache]
    public sealed class SessionGlobal
    {
        private const string EMPTY_KEY = "__NULL_";

        private SessionGlobal()
        {
        }

        public string SessionId { get; set; }

        public IUserInfo Info
        {
            get
            {
                return BaseGlobalVariable.Current.UserInfo;
            }
        }

        public Guid TempIndentity { get; private set; }

        public WebAppRight AppRight { get; private set; }

        public void SetGuid()
        {
            TempIndentity = Guid.NewGuid();
        }

        private void Initialize(string userId)
        {
            TempIndentity = Guid.NewGuid();
            var appRightBuilder = PlugInFactoryManager.CreateInstance<IAppRightBuilder>(
                AppRightBuilderPlugInFactory.REG_NAME, WebAppSetting.WebCurrent.AppRightBuilder);
            AppRight = new WebAppRight(appRightBuilder);
            SessionId = WebGlobalVariable.Session?.Id;

            if (userId != EMPTY_KEY)
                AppRight.Initialize(Info);
        }

        internal static SessionGlobal CreateSessionGlobal(string userId)
        {
            SessionGlobal global = new SessionGlobal();
            global.Initialize(userId);

            return global;
        }

        public static void Abandon(IUserInfo info)
        {
            TkDebug.AssertArgumentNull(info, nameof(info), null);

            string userId = info.UserId.ConvertToString();
            if (string.IsNullOrEmpty(userId))
                return;

            CacheManager.RemoveKey("SessionGlobal", userId);
        }

        public static SessionGlobal GetSessionGlobal(IUserInfo info)
        {
            TkDebug.AssertArgumentNull(info, nameof(info), null);

            string userId = info.UserId.ConvertToString();
            if (string.IsNullOrEmpty(userId))
                userId = EMPTY_KEY;
            var global = CacheManager.GetItem("SessionGlobal", userId).Convert<SessionGlobal>();

            return global;
        }
    }
}