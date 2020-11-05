using System;
using System.Web;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Sys
{
    public class ReLogonExceptionHandler : IExceptionHandler
    {
        public static readonly IExceptionHandler Handler = new ReLogonExceptionHandler();

        private ReLogonExceptionHandler()
        {
        }

        #region IExceptionHandler 成员

        public void HandleException(IWebHandler handler, WebBasePage page, Exception ex)
        {
            TkDebug.AssertArgument(ex is ReLogOnException, "ex", string.Format(ObjectUtil.SysCulture,
                "此Handler只处理ReLogonException，当前的Exception类型为{0}", ex.GetType()), this);
            TkDebug.ThrowIfNoAppSetting();

            string logOnUrl = WebAppSetting.WebCurrent.LogOnPath;
            UriBuilder builder = new UriBuilder(logOnUrl);
            string retUrl = "RetUrl=" + HttpUtility.UrlEncode(ex.Message);
            if (builder.Query != null && builder.Query.Length > 1)
                builder.Query = builder.Query.Substring(1) + "&" + retUrl;
            else
                builder.Query = retUrl;
            handler.Response.Redirect(builder.Uri.ToString(), false);
        }

        #endregion
    }
}
