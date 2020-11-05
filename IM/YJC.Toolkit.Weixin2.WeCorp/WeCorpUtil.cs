using System.Threading;
using YJC.Toolkit.Cache;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Threading;

namespace YJC.Toolkit.WeCorp
{
    public static class WeCorpUtil
    {
        internal static string GetAccessToken(string tenantId, string appName)
        {
            AccessToken token = CacheManager.GetItem("WeCorpToken",
                GetKey(tenantId, appName), null).Convert<AccessToken>();
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
                return "没有配置微信企业号的参数，请确认配置相应的CorpId，CorpSecret等参数";
            return string.Format(ObjectUtil.SysCulture,
                "没有为租户{0}配置微信企业号的参数，请确认配置相应的CorpId，CorpSecret等参数", tenantId);
        }

        internal static AccessToken CreateToken(string tenantId, string appName)
        {
            WeCorpSettings setting = WeCorpConfiguration.Create(tenantId);
            TkDebug.AssertNotNull(setting, GetErrorMessage(tenantId), null);
            var service = GetWeCorpService<ICorpConnectionService>(tenantId);
            AccessToken token = service.GetAccessToken(setting.CorpId,
                setting.GetSecret(appName));
            token.SetExpireTime();
            WeCorpConfiguration.SaveToken(tenantId, appName, token);
            return token;
        }

        private static string GetKey(string tenantId, string appName)
        {
            WeCorpCacheKey key = new WeCorpCacheKey(tenantId, appName);
            return key.ToJson();
        }

        public static T GetWeCorpService<T>() where T : class
        {
            return GetWeCorpService<T>(null, null);
        }

        public static T GetWeCorpService<T>(string appName) where T : class
        {
            return GetWeCorpService<T>(appName, null);
        }

        public static T GetWeCorpService<T>(string appName, string tenantId) where T : class
        {
            IIMPlatform platform = new WeCorpPlatform(appName, tenantId);
            return IMUtil.GetService<T>(platform);
        }
    }
}