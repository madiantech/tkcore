using System.Threading;
using YJC.Toolkit.Cache;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Threading;
using YJC.Toolkit.WeChat.Service;

namespace YJC.Toolkit.WeChat
{
    public static class WeChatUtil
    {
        public const string TENANT_ID = "TenantId";

        internal static string GetAccessToken(string tenantId)
        {
            AccessToken token = CacheManager.GetItem("WeChatToken",
                GetKey(tenantId), null).Convert<AccessToken>();
            ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
            using (rwLock)
            {
                token = rwLock.ReadLockAction(() =>
               {
                   if (!token.IsValid)
                   {
                       token = rwLock.WriteLockAction(() =>
                       {
                           AccessToken newToken = CreateToken(tenantId);
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
                return "没有配置微信公众号的参数，请确认配置相应的AppId，AppSecret等参数";
            return string.Format(ObjectUtil.SysCulture,
                "没有为租户{0}配置微信公众号的参数，请确认配置相应的AppId，AppSecret等参数", tenantId);
        }

        internal static AccessToken CreateToken(string tenantId)
        {
            WeChatSettings setting = WeChatConfiguration.Create(tenantId);
            TkDebug.AssertNotNull(setting, GetErrorMessage(tenantId), null);
            var service = GetWeChatService<IConnectionService>(tenantId);
            AccessToken token = service.GetAccessToken(setting.AppId, setting.AppSecret);
            token.SetExpireTime();
            //token.ExpiresIn = 7200;
            //token.SetExpireTime();
            WeChatConfiguration.SaveToken(tenantId, token);
            return token;
        }

        private static string GetKey(string tenantId)
        {
            return string.IsNullOrEmpty(tenantId) ? IMUtil.NULL_KEY : tenantId;
        }

        public static T GetWeChatService<T>() where T : class
        {
            return GetWeChatService<T>(null);
        }

        public static T GetWeChatService<T>(string tenantId) where T : class
        {
            IIMPlatform platform = new WeChatPlatform(tenantId);
            return IMUtil.GetService<T>(platform);
        }
    }
}