using YJC.Toolkit.Sys;

namespace YJC.Toolkit.IM
{
    public interface ICorpConnectionService
    {
        [ApiMethod("/gettoken", UseAccessToken = false)]
        AccessToken GetAccessToken([ApiParameter(NamingRule = NamingRule.Lower)]string corpId,
            [ApiParameter(NamingRule = NamingRule.Lower)]string corpSecret);
    }
}
