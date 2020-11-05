using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Service
{
    public interface IConnectionService
    {
        [ApiMethod("/gettoken", UseAccessToken = false)]
        AccessToken GetAccessToken([ApiParameter(NamingRule = NamingRule.Lower)]string appKey,
            [ApiParameter(NamingRule = NamingRule.Lower)]string appSecret);
    }
}