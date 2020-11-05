using YJC.Toolkit.Collections;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeCorp
{
    public class WeCorpSettings
    {
        private readonly RegNameList<WeCorpAppConfig> fCorpApps;

        public WeCorpSettings(string corpId, string userManagerSecret, string chatSecret)
        {
            TkDebug.AssertArgumentNullOrEmpty(corpId, "corpId", null);
            TkDebug.AssertArgumentNullOrEmpty(userManagerSecret, "userManagerSecret", null);
            TkDebug.AssertArgumentNullOrEmpty(chatSecret, "chatSecret", null);

            CorpId = corpId;
            UserManagerSecret = userManagerSecret;
            ChatSecret = chatSecret;
            fCorpApps = new RegNameList<WeCorpAppConfig>();
        }

        public string CorpId { get; protected set; }

        public string UserManagerSecret { get; protected set; }

        public string ChatSecret { get; protected set; }

        public string MenuSecret { get; set; }

        internal RegNameList<WeCorpAppConfig> CorpApps => fCorpApps;

        private string GetMenuSecret()
        {
            return string.IsNullOrEmpty(MenuSecret) ? UserManagerSecret : MenuSecret;
        }

        public void Add(WeCorpAppConfig config)
        {
            fCorpApps.Add(config);
        }

        public string GetSecret(string appName)
        {
            if (string.IsNullOrEmpty(appName))
                appName = IMConst.WECORP_USER_MANAGER;

            switch (appName)
            {
                case IMConst.WECORP_USER_MANAGER:
                    return UserManagerSecret;

                case IMConst.WECORP_MENU:
                    return GetMenuSecret();

                case IMConst.WECORP_CHAT:
                    return ChatSecret;

                default:
                    WeCorpAppConfig config = GetAppConfig(appName);
                    return config.Secret;
            }
        }

        public WeCorpAppConfig GetAppConfig(string appName)
        {
            TkDebug.AssertArgumentNullOrEmpty(appName, nameof(appName), this);

            var config = fCorpApps[appName];
            TkDebug.AssertNotNull(config, $"没有为App {appName}配置相应的参数", this);
            return config;
        }

        public int GetAgentId(string appName) => GetAppConfig(appName).AppId;
    }
}