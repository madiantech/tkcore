using System;
using YJC.Toolkit.IM;

namespace YJC.Toolkit.WeChat
{
    internal class WeChatPlatform : IIMPlatform
    {
        public WeChatPlatform(string tenantId)
        {
            TenantId = tenantId;
        }

        #region IIMPlatform 成员

        public Uri BaseUri
        {
            get
            {
                return IMConst.BASE_WECHAT_URL;
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
                return WeChatUtil.GetAccessToken(TenantId);
            }
        }

        #endregion IIMPlatform 成员

        public string TenantId { get; private set; }

        public override string ToString()
        {
            return "微信公众号";
        }
    }
}