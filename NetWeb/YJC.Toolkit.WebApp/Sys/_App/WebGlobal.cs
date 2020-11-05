using System;
using System.Collections.Specialized;
using System.Threading;
using System.Web;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Sys
{
    public class WebGlobal : HttpApplication, IWebHandler
    {
        private IQueryString fQueryString;

        private void Application_Start(object sender, EventArgs e)
        {
            // 在应用程序启动时运行的代码
            WebApp.ApplicationStart(this);
        }

        private void Application_End(object sender, EventArgs e)
        {
            //  在应用程序关闭时运行的代码
            WebApp.ApplicationEnd(this);
        }

        private void Application_Error(object sender, EventArgs e)
        {
            // 在出现未处理的错误时运行的代码
            Exception ex = Server.GetLastError();
            if (ex is ThreadAbortException)
                return;

            WebAppSetting appSetting = WebAppSetting.WebCurrent;
            //if (ex is ReLogOnException)
            //    ExceptionUtil.HandleException(appSetting.ReLogOnHandler, this, null, ex);
            //else if (ex is ErrorPageException)
            //    ExceptionUtil.HandleException(appSetting.ErrorPageHandler, this, null, ex);
            //else if (ex is ErrorOperationException)
            //    ExceptionUtil.HandleException(appSetting.ErrorOpeartionHandler, this, null, ex);
            //else if (ex is ToolkitException)
            //    ExceptionUtil.HandleException(appSetting.ToolkitHandler, this, null, ex);
            //else
            //    ExceptionUtil.HandleException(appSetting.ExceptionHandler, this, null, ex);
            Server.ClearError();
        }

        private void Session_Start(object sender, EventArgs e)
        {
            // 在新会话启动时运行的代码
            SessionGlobal global = WebApp.NewDefaultSessionGlobal(Session);
            WebApp.SessionStart(this, global);
        }

        private void Session_End(object sender, EventArgs e)
        {
            // 在会话结束时运行的代码。
            // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
            // InProc 时，才会引发 Session_End 事件。如果会话模式设置为 StateServer
            // 或 SQLServer，则不会引发该事件。
            WebApp.SessionEnd(this);
        }

        #region IPageData 成员

        public string Title
        {
            get
            {
                return string.Empty;
            }
        }

        public Uri PageUrl
        {
            get
            {
                return Request.Url;
            }
        }

        public string PageExtension
        {
            get
            {
                return string.Empty;
            }
        }

        public bool IsMobileDevice
        {
            get
            {
                return Request.Browser.IsMobileDevice;
            }
        }

        #endregion IPageData 成员

        #region IInputData 成员

        public IPageStyle Style
        {
            get
            {
                return Request.QueryString["Style"].Value<PageStyleClass>(PageStyleClass.Empty);
            }
        }

        public bool IsPost
        {
            get
            {
                return Request.HttpMethod == "POST";
            }
        }

        public IQueryString QueryString
        {
            get
            {
                if (fQueryString == null)
                    fQueryString = new QueryStringWrapper(Request.QueryString, Request.Headers);
                return fQueryString;
            }
        }

        public object PostObject
        {
            get
            {
                return null;
            }
        }

        public ICallerInfo CallerInfo
        {
            get
            {
                return null;
            }
        }

        public string QueryStringText
        {
            get
            {
                return WebUtil.GetQueryString(Request.Url);
            }
        }

        public PageSourceInfo SourceInfo
        {
            get
            {
                var info = WebUtil.CreateSourceInfo(Request);
                return info;
            }
        }

        #endregion IInputData 成员
    }
}