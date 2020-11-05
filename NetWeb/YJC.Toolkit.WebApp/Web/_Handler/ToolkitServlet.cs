using System;
using System.Collections.Specialized;
using System.Threading;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public abstract class ToolkitServlet : System.Web.UI.Page, IPageData, IWebHandler
    {
        private InternalCallerInfo fCallerInfo;
        private IQueryString fQueryString;
        private PageSourceInfo fSourceInfo;

        /// <summary>
        /// Initializes a new instance of the ToolkitServlet class.
        /// </summary>
        protected ToolkitServlet()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            string retUrl = ObjectUtil.GetDefaultValue(Request.QueryString["RetUrl"], string.Empty);
            Uri retUri = string.IsNullOrEmpty(retUrl) ? null : new Uri(retUrl, UriKind.RelativeOrAbsolute);
            SessionGlobal sessionGbl = (SessionGlobal)Session[WebGlobalVariable.SESSION_DATA];
            fCallerInfo = new InternalCallerInfo(this, sessionGbl, retUri, Request.Url);
            Load += Page_Load;
        }

        private void Page_Load(object sender, EventArgs e)
        {
            IsPost = Request.HttpMethod == "POST";

            WebBasePage page = null;
            WebDefaultXmlConfig defaultConfig = WebGlobalVariable.WebCurrent.WebDefaultValue;
            try
            {
                using (page = CreatePage())
                {
                    if (page != null)
                    {
                        page.LoadPage();
                    }
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

        protected abstract WebBasePage CreatePage();

        #region IPageData 成员

        string IPageData.Title
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
        }

        public bool IsPost { get; set; }

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
    }
}