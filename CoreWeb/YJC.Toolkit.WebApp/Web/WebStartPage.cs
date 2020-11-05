using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal static class WebStartPage
    {
        public static bool IsDefault(HttpContext context)
        {
            return context.Request.Path.Value == "/";
        }

        public static Task Start(HttpContext context)
        {
            IUserInfo info = BaseGlobalVariable.Current.UserInfo;
            WebAppSetting appSetting = WebAppSetting.WebCurrent;

            string url;
            if (info.IsLogOn)
            {
                if (string.IsNullOrEmpty(appSetting.MainPath))
                    url = appSetting.HomePath;
                else
                {
                    url = HttpUtility.UrlEncode(AppUtil.ResolveUrl(appSetting.HomePath));
                    url = UriUtil.AppendQueryString(appSetting.MainPath, "StartUrl=" + url);
                }
            }
            else
                url = appSetting.StartupPath;
            url = AppUtil.ResolveUrl(url);
            context.Response.Redirect(url);

            return Task.FromResult(0);
        }
    }
}