using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeChat.Service
{
    public interface IConnectionService
    {
        [ApiMethod("/token?grant_type=client_credential", UseAccessToken = false)]
        AccessToken GetAccessToken([ApiParameter(NamingRule = NamingRule.Lower)]string appId,
            [ApiParameter(NamingRule = NamingRule.Lower)]string secret);
    }
}