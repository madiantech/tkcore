using System.Threading;
using YJC.Toolkit.Cache;
using YJC.Toolkit.DingTalk.Model;
using YJC.Toolkit.DingTalk.Service;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Threading;

namespace YJC.Toolkit.DingTalk
{
    public static class DingTalkUtil
    {
        internal static string GetAccessToken(string tenantId, string appName)
        {
            AccessToken token = CacheManager.GetItem("DingTalkToken",
               AppCacheKey.GetKey(tenantId, appName), null).Convert<AccessToken>();
            ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
            using (rwLock)
            {
                token = rwLock.ReadLockAction(() =>
                {
                    if (!token.IsValid)
                    {
                        token = rwLock.WriteLockAction(() =>
                        {
                            AccessToken newToken = CreateToken(tenantId, appName);
                            token.Assign(newToken);
                            return token;
                        });
                    }
                    return token;
                });
                return token.Token;
            }
        }

        private static string GetErrorMessage(string tenantId)
        {
            if (string.IsNullOrEmpty(tenantId))
                return "没有配置钉钉的参数，请确认配置相应的CorpId，CorpSecret等参数";
            return string.Format(ObjectUtil.SysCulture,
                "没有为租户{0}配置钉钉的参数，请确认配置相应的CorpId，CorpSecret等参数", tenantId);
        }

        internal static AccessToken CreateToken(string tenantId, string appName)
        {
            DingTalkAppConfig config = GetAppConfig(tenantId, appName);
            var service = GetDingTalkService<IConnectionService>(tenantId, appName);

            AccessToken token = service.GetAccessToken(config.AppKey, config.AppSecret);
            token.SetExpireTime(7200);
            //token.ExpiresIn = 7200;
            //token.SetExpireTime();
            DingTalkConfiguration.SaveToken(tenantId, appName, token);
            return token;
        }

        public static T GetDingTalkService<T>(string appName) where T : class
        {
            return GetDingTalkService<T>(null, appName);
        }

        public static T GetDingTalkService<T>(string tenantId, string appName) where T : class
        {
            TkDebug.AssertArgumentNullOrEmpty(appName, "appName", null);

            IIMPlatform platform = new DingTalkPlatform(tenantId, appName);

            return IMUtil.GetService<T>(platform);
        }

        public static DingTalkAppConfig GetAppConfig(string appName)
        {
            return GetAppConfig(null, appName);
        }

        public static DingTalkAppConfig GetAppConfig(string tenantId, string appName)
        {
            TkDebug.AssertArgumentNullOrEmpty(appName, "appName", null);

            DingTalkSettings setting = GetSetting(tenantId);

            DingTalkAppConfig config = setting[appName];
            TkDebug.AssertNotNull(config, string.Format(ObjectUtil.SysCulture,
                "没有配置应用{0}", appName), null);

            return config;
        }

        private static DingTalkSettings GetSetting(string tenantId)
        {
            DingTalkSettings setting = DingTalkConfiguration.Create(tenantId);
            TkDebug.AssertNotNull(setting, GetErrorMessage(tenantId), null);
            return setting;
        }

        public static string GetAppName(string appName)
        {
            return GetAppName(null, appName);
        }

        public static string GetAppName(string tenantId, string appName)
        {
            if (!string.IsNullOrEmpty(appName))
                return appName;

            DingTalkSettings setting = GetSetting(tenantId);
            return setting.DefaultAppName;
        }

        public static FileType GetFileType(string extension)
        {
            if (string.IsNullOrEmpty(extension))
                return FileType.File;

            extension = extension.ToLower(ObjectUtil.SysCulture);
            if (extension == "jpg" || extension == "jpeg")
                return FileType.Image;
            if (extension == "amr")
                return FileType.Voice;

            return FileType.File;
        }
    }
}