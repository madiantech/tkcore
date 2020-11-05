using System;
using YJC.Toolkit.IM;

namespace YJC.Toolkit.DingTalk
{
    internal class DingTalkPlatform : IIMPlatform
    {
        public DingTalkPlatform(string tenantId, string appName)
        {
            AppName = appName;
            TenantId = tenantId;
        }

        #region IIMPlatform 成员

        public Uri BaseUri
        {
            get
            {
                return IMConst.BASE_DINGTALK_URL;
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
                return DingTalkUtil.GetAccessToken(TenantId, AppName);
            }
        }

        #endregion IIMPlatform 成员

        public string TenantId { get; private set; }

        public string AppName { get; private set; }

        public override string ToString()
        {
            return "钉钉";
        }
    }
}