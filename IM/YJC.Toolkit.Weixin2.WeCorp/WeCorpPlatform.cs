using System;
using YJC.Toolkit.IM;

namespace YJC.Toolkit.WeCorp
{
    internal class WeCorpPlatform : IIMPlatform
    {
        public WeCorpPlatform(string appName, string tenantId)
        {
            if (string.IsNullOrEmpty(appName))
                AppName = IMConst.WECORP_USER_MANAGER;

            AppName = appName;
            TenantId = tenantId;
        }

        #region IIMPlatform 成员

        public Uri BaseUri
        {
            get
            {
                return IMConst.BASE_WECORP_URL;
            }
        }

        public string QueryStringName
        {
            get
            {
                return IMConst.ACCESS_TOKEN_NAME;
            }
        }

        public string AccessToken
        {
            get
            {
                return WeCorpUtil.GetAccessToken(TenantId, AppName);
            }
        }

        #endregion IIMPlatform 成员

        public string TenantId { get; private set; }

        public string AppName { get; private set; }

        public override string ToString()
        {
            return "微信企业号";
        }
    }
}