using System;
using System.Collections.Generic;
using System.Web;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleRight
{
    [Source(Author = "YJC", CreateDate = "2014-07-10", Description = "主界面")]
    internal class MainPageSource : ISource
    {
        #region ISource 成员

        public OutputData DoAction(IInputData input)
        {
            IUserInfo userInfo = BaseGlobalVariable.Current.UserInfo;

            Dictionary<string, string> result = new Dictionary<string, string>();
            string mainPage = WebAppSetting.WebCurrent.MainPath;

            result["Menu"] = WebGlobalVariable.SessionGbl.AppRight.CreateMenu(userInfo);
            result["UserName"] = userInfo.UserName;
            result["StartUrl"] = WebUtil.ResolveUrl(input.QueryString["StartUrl"]);
            result["HomeUrl"] = WebUtil.ResolveUrl(UriUtil.AppendQueryString(mainPage, "StartUrl="
                + HttpUtility.UrlEncode(WebAppSetting.WebCurrent.HomePath)));
            result["FullName"] = WebAppSetting.WebCurrent.AppFullName;
            result["ShortName"] = WebAppSetting.WebCurrent.AppShortName;

            return OutputData.CreateObject(result);
        }

        #endregion ISource 成员
    }
}