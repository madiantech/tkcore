using System;
using System.Web;
using YJC.Toolkit.Data;
using YJC.Toolkit.LogOn;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleRight
{
    [Serializable]
    internal class UserLogOnRight : EmptyLogOnRight
    {
        public override void Initialize(IUserInfo user)
        {
        }

        public override void CheckLogOn(object userId, Uri url)
        {
            string logOnPage = WebAppSetting.WebCurrent.LogOnPath;

            if (!string.IsNullOrEmpty(logOnPage) && !BaseGlobalVariable.Current.UserInfo.IsLogOn)
            {
                logOnPage = UriUtil.AppendQueryString(logOnPage,
                    "RetUrl=" + HttpUtility.UrlEncode(url.ToString()));
                throw new RedirectException(logOnPage);
            }
        }

        //private static bool RetriveSessionFromCookie()
        //{
        //    var cookies = WebGlobalVariable.Request.Cookies;
        //    HttpCookie cookie = cookies[RightConst.USER_INFO_COOKIE_NAME];
        //    if (cookie == null)
        //        return false;

        //    CookieUserInfo userInfo = CookieUserInfo.FromEncodeString(cookie.Value);
        //    if (userInfo == null)
        //        return false;

        //    EmptyDbDataSource source = new EmptyDbDataSource();
        //    using (source)
        //    using (UserResolver resolver = new UserResolver(source))
        //    {
        //        try
        //        {
        //            IUserInfo info = resolver.CheckUserLogOnById(userInfo.UserId, userInfo.Password);
        //            WebGlobalVariable.SessionGbl.AppRight.Initialize(info);
        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }
        //    }
        //}
    }
}