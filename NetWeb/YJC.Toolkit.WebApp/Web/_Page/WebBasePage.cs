using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web;
using System.Web.SessionState;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public abstract class WebBasePage : IWebHandler, IPageData, IDisposable
    {
        private bool fSupportLogOn;
        private ISource fSource;
        private InternalCallerInfo fCallerInfo;
        private readonly PageSourceInfo fPageSourceInfo;

        protected WebBasePage()
        {
            WebGlobalVariable.AssertHttpContext();
            HttpContext context = HttpContext.Current;
            Application = context.Application;
            Session = context.Session;
            Server = context.Server;
            Request = context.Request;
            Response = context.Response;
            QueryString = new QueryStringWrapper(Request.QueryString, Request.Headers);
            SessionGbl = Session[WebGlobalVariable.SESSION_DATA] as SessionGlobal;
            IsPost = Request.HttpMethod == "POST";
            fSupportLogOn = true;

            SelfUrl = InternalUriUtil.GetSelfUrl(Request.Url);
            string retUrl = ObjectUtil.GetDefaultValue(Request.QueryString["RetUrl"], string.Empty);
            RetUrl = string.IsNullOrEmpty(retUrl) ? null : new Uri(retUrl, UriKind.RelativeOrAbsolute);
            fPageSourceInfo = WebUtil.CreateSourceInfo(Request);
            //Style = fPageSourceInfo.Style;
        }

        #region IWebHandler 成员

        public HttpRequest Request { get; private set; }

        public HttpResponse Response { get; private set; }

        public HttpSessionState Session { get; private set; }

        #endregion IWebHandler 成员

        #region IPageData 成员

        public virtual string Title
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
                return fPageSourceInfo.Style;
            }
            protected set
            {
                TkDebug.AssertArgumentNull(value, "value", this);

                fPageSourceInfo.Style = value;
            }
        }

        public PageSourceInfo SourceInfo
        {
            get
            {
                return fPageSourceInfo;
            }
        }

        public bool IsPost { get; private set; }

        public IQueryString QueryString { get; private set; }

        public object PostObject { get; private set; }

        public virtual ICallerInfo CallerInfo
        {
            get
            {
                if (fCallerInfo == null)
                    fCallerInfo = new InternalCallerInfo(this, SessionGbl, RetUrl, SelfUrl);
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

        #region IDisposable 成员

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable 成员

        public HttpApplicationState Application { get; private set; }

        public HttpServerUtility Server { get; private set; }

        public SessionGlobal SessionGbl { get; private set; }

        public Uri RetUrl { get; private set; }

        public Uri SelfUrl { get; private set; }

        protected virtual bool SupportLogOn
        {
            get
            {
                return fSupportLogOn;
            }
            set
            {
                fSupportLogOn = value;
            }
        }

        protected virtual IMetaData MetaData
        {
            get
            {
                return null;
            }
        }

        internal PageSourceInfo UrlInfo
        {
            get
            {
                return fPageSourceInfo;
            }
        }

        protected virtual bool DisableInjectCheck { get; set; }

        protected abstract IPostObjectCreator PostObjectCreator { get; }

        public abstract ISource Source
        {
            get;
        }

        private void SetSource(ISource value)
        {
            fSource = value;
            if (value == null)
                return;
            SourceWebPageAttribute attribute = Attribute.GetCustomAttribute(value.GetType(),
                typeof(SourceWebPageAttribute), false).Convert<SourceWebPageAttribute>();
            if (attribute != null)
            {
                SupportLogOn = attribute.SupportLogOn;
                DisableInjectCheck = attribute.DisableInjectCheck;
            }
            OnSourceCreated(value);
        }

        private void Post()
        {
            PreparePostData();
            DoPost();
        }

        private void CheckFunctionRight()
        {
            ISupportFunction functionSource = Source as ISupportFunction;
            if (functionSource != null)
            {
                IFunctionRight functionRight = SessionGbl.AppRight.FunctionRight;
                switch (functionSource.FunctionType)
                {
                    case FunctionRightType.Admin:
                        if (!functionRight.IsAdmin())
                            throw new NoFunctionRightException();
                        break;

                    case FunctionRightType.Function:
                        if (!functionRight.IsFunction(functionSource.FunctionKey))
                            throw new NoFunctionRightException();
                        break;

                    case FunctionRightType.SubFunction:
                        string subKey = GetSubFunctionKey();
                        if (!functionRight.IsSubFunction(subKey, functionSource.FunctionKey))
                            throw new NoFunctionRightException();
                        break;

                    case FunctionRightType.None:
                        break;
                }
            }
        }

        private string GetSubFunctionKey()
        {
            return Style.ToString();
        }

        protected virtual void PreparePostData()
        {
            try
            {
                IPostObjectCreator creator = PostObjectCreator;
                if (creator == null)
                    PostObject = null;
                else
                    PostObject = creator.Read(this, Request.InputStream);
            }
            catch (Exception ex)
            {
                TkDebug.ThrowIfNoAppSetting();
                if (BaseAppSetting.Current.IsDebug)
                {
                    string file = Path.Combine(BaseAppSetting.Current.XmlPath, "Post.txt");
                    byte[] data = new byte[Request.InputStream.Length];
                    Request.InputStream.Position = 0;
                    Request.InputStream.Read(data, 0, data.Length);
                    string content = Encoding.UTF8.GetString(data);

                    TkTrace.WriteFile(file, content);
                }
                TkDebug.ThrowToolkitException("提交上来的数据不是XML，如果Debug开启，可以查看Post.txt中的内容", ex, this);
            }
        }

        protected virtual void OnSourceCreated(ISource source)
        {
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                PostObject.DisposeObject();
                fSource.DisposeObject();
            }
        }

        protected virtual void Log(ISource source, OutputData output)
        {
        }

        protected abstract void DoGet();

        protected abstract void DoPost();

        protected virtual bool HandleException(Exception ex)
        {
            return false;
        }

        internal void LoadPage()
        {
            if (SupportLogOn)
                SessionGbl.AppRight.LogOnRight.CheckLogOn(SessionGbl.Info.UserId, SelfUrl);

            CheckFunctionRight();
            try
            {
                if (IsPost)
                    Post();
                else
                    DoGet();
            }
            catch (Exception ex)
            {
                if (!HandleException(ex))
                    throw new Exception(ex.Message, ex);
            }
        }
    }
}