using System;
using System.Collections.Specialized;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using YJC.Toolkit.Cache;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public abstract class ToolkitHttpHandler : IHttpHandler, IRequiresSessionState,
        ICacheDependencyCreator, IPageData, IWebHandler, IDisposable
    {
        private ICacheDependency fDependency;
        private HttpContext fContext;
        private PageSourceInfo fSourceInfo;
        private InternalCallerInfo fCallerInfo;
        private IQueryString fQueryString;

        protected ToolkitHttpHandler()
        {
        }

        #region IHttpHandler 成员

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            fContext = context;
            IsPost = Request.HttpMethod == "POST";
            SessionGlobal sessionGbl = (SessionGlobal)Session[WebGlobalVariable.SESSION_DATA];
            string retUrl = ObjectUtil.GetDefaultValue(Request.QueryString["RetUrl"], string.Empty);
            Uri retUri = string.IsNullOrEmpty(retUrl) ? null : new Uri(retUrl, UriKind.RelativeOrAbsolute);
            fCallerInfo = new InternalCallerInfo(this, sessionGbl, retUri, Request.Url);

            WebBasePage page = null;
            WebDefaultXmlConfig defaultConfig = WebGlobalVariable.WebCurrent.WebDefaultValue;
            try
            {
                try
                {
                    page = CreatePage();
                    if (page != null)
                    {
                        page.LoadPage();
                    }
                }
                catch (RedirectException ex)
                {
                    Response.Redirect(ex.Url.ToString(), false);
                }
                catch (ReLogOnException ex)
                {
                    TkDebug.ThrowIfNoAppSetting();
                    ExceptionUtil.HandleException(defaultConfig.ReLogOnHandler,
                        this, page, ex);
                }
                catch (Exception ex)
                {
                    if (ex is ThreadAbortException)
                        return;
                    TkDebug.ThrowIfNoAppSetting();
                    Exception innerEx = ex.InnerException;
                    if (innerEx != null)
                    {
                        if (innerEx is RedirectException)
                            Response.Redirect(((RedirectException)innerEx).Url.ToString(), false);
                        else if (innerEx is ReLogOnException)
                            ExceptionUtil.HandleException(defaultConfig.ReLogOnHandler,
                                this, page, innerEx);
                        else if (innerEx is ErrorPageException)
                            ExceptionUtil.HandleException(defaultConfig.ErrorPageHandler,
                                this, page, innerEx);
                        else if (innerEx is ErrorOperationException)
                            ExceptionUtil.HandleException(defaultConfig.ErrorOperationHandler,
                                this, page, innerEx);
                        else if (innerEx is ToolkitException)
                            ExceptionUtil.HandleException(defaultConfig.ToolkitHandler,
                                this, page, innerEx);
                        else
                            ExceptionUtil.HandleException(defaultConfig.ExceptionHandler,
                                this, page, innerEx);
                    }
                    else
                        ExceptionUtil.HandleException(defaultConfig.ExceptionHandler,
                            this, page, ex);
                }
            }
            finally
            {
                page.DisposeObject();
            }
        }

        #endregion IHttpHandler 成员

        #region IDisposable 成员

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable 成员

        #region ICacheDependencyCreator 成员

        ICacheDependency ICacheDependencyCreator.CreateCacheDependency()
        {
            return CreateCacheDependency();
        }

        #endregion ICacheDependencyCreator 成员

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
                return InternalUriUtil.GetUrlExtension(Request.Url);
            }
        }

        public bool IsMobileDevice
        {
            get
            {
                return Request.Browser.IsMobileDevice;
            }
        }

        public IPageStyle Style
        {
            get
            {
                return SourceInfo.Style;
            }
        }

        public PageSourceInfo SourceInfo
        {
            get
            {
                if (fSourceInfo == null)
                    fSourceInfo = WebUtil.CreateSourceInfo(Request);
                return fSourceInfo;
            }
            internal set
            {
                fSourceInfo = value;
            }
        }

        public bool IsPost { get; private set; }

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
                return fCallerInfo;
            }
        }

        public string QueryStringText
        {
            get
            {
                return WebUtil.GetQueryString(Request.Url);
            }
        }

        #endregion IPageData 成员

        public HttpContext Context
        {
            get
            {
                return fContext;
            }
        }

        public HttpApplicationState Application
        {
            get
            {
                return fContext.Application;
            }
        }

        public HttpSessionState Session
        {
            get
            {
                return fContext.Session;
            }
        }

        public HttpServerUtility Server
        {
            get
            {
                return fContext.Server;
            }
        }

        public HttpRequest Request
        {
            get
            {
                return fContext.Request;
            }
        }

        public HttpResponse Response
        {
            get
            {
                return fContext.Response;
            }
        }

        protected internal ICacheDependency CreateCacheDependency()
        {
            return fDependency;
        }

        internal void SetTkFileName(string fileName)
        {
            fDependency = new FileDependency(fileName);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                fDependency.DisposeObject();
        }

        protected abstract WebBasePage CreatePage();
    }
}