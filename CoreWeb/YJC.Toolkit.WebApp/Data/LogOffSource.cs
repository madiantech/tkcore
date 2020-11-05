using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Data
{
    [Source(Author = "YJC", Description = "退出系统", CreateDate = "2014-02-25")]
    [OutputRedirector]
    public class LogOffSource : ISource
    {
        #region ISource 成员

        public OutputData DoAction(IInputData input)
        {
            WebGlobalVariable.Session?.Clear();
            WebGlobalVariable.Response.Cookies.Delete(JWTUtil.COOKIE_NAME);
            SessionGlobal.Abandon(BaseGlobalVariable.Current.UserInfo);

            string url = WebAppSetting.WebCurrent.LogOnPath;
            return OutputData.Create(url);
        }

        #endregion ISource 成员
    }
}