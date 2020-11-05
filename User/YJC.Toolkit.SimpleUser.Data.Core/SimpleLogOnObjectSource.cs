using System;
using System.Data;
using System.Web;
using YJC.Toolkit.Data;
using YJC.Toolkit.LogOn;
using YJC.Toolkit.Sys;
using Microsoft.AspNetCore.Http;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.SimpleRight
{
    [ObjectSource(Author = "YJC", CreateDate = "2014-12-03", Description = "常规的用户登录（即用户名+密码）")]
    internal sealed class SimpleLogOnObjectSource : IInsertObjectSource, IDisposable, IDbDataSource
    {
        private const string COOKIE_NAME = "LogOnName";

        public SimpleLogOnObjectSource()
        {
            DataSet = new DataSet(ToolkitConst.TOOLKIT) { Locale = ObjectUtil.SysCulture };
            Context = DbContextUtil.CreateDefault();
        }

        #region IObjectInsertSource 成员

        public object CreateNew(IInputData input)
        {
            LogOnData data = new LogOnData();
            var request = WebGlobalVariable.Request;
            var logonName = request.Cookies[COOKIE_NAME];
            if (logonName != null && !string.IsNullOrEmpty(logonName))
                data.LogOnName = logonName;
            return data;
        }

        public OutputData Insert(IInputData input, object instance)
        {
            LogOnData data = instance.Convert<LogOnData>();
            using (UserResolver resolver = new UserResolver(this))
            {
                IUserInfo userInfo = resolver.CheckUserLogOn(data.LogOnName, data.Password, 0);
                if (userInfo == null)
                {
                }

                var response = WebGlobalVariable.Response;
                CookieOptions options = new CookieOptions
                {
                    Expires = new DateTimeOffset(DateTime.Now.AddDays(30))
                };
                response.Cookies.Append(COOKIE_NAME, data.LogOnName, options);
                options = new CookieOptions
                {
                    Expires = new DateTimeOffset(JWTUtil.CalcValidTime())
                };
                string token = JWTUtil.CreateEncodingInfo(userInfo);
                response.Cookies.Append(JWTUtil.COOKIE_NAME, token, options);
                //CookieUserInfo cookieInfo = new CookieUserInfo(data, userInfo);
                //cookie = new HttpCookie(RightConst.USER_INFO_COOKIE_NAME, cookieInfo.Encode())
                //{
                //    Expires = GetExpireDate()
                //};
                //response.Cookies.Set(cookie);

                WebSuccessResult result;
                string retUrl = input.QueryString["RetURL"];
                if (!string.IsNullOrEmpty(retUrl))
                    result = new WebSuccessResult(retUrl);
                else
                {
                    WebAppSetting appSetting = WebAppSetting.WebCurrent;
                    if (string.IsNullOrEmpty(appSetting.MainPath))
                        result = new WebSuccessResult(appSetting.HomePath);
                    else
                    {
                        string url = HttpUtility.UrlEncode(appSetting.HomePath);
                        string mainUrl = UriUtil.AppendQueryString(appSetting.MainPath, "StartUrl=" + url);
                        result = new WebSuccessResult(mainUrl);
                    }
                }

                return OutputData.CreateToolkitObject(result);
            }
        }

        #endregion IObjectInsertSource 成员

        #region IDisposable 成员

        public void Dispose()
        {
            DataSet.Dispose();
            Context.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable 成员

        #region IDbDataSource 成员

        public DataSet DataSet { get; private set; }

        public TkDbContext Context { get; private set; }

        #endregion IDbDataSource 成员
    }
}