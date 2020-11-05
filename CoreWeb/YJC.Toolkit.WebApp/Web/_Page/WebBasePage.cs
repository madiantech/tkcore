using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using YJC.Toolkit.Data;
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
        private Uri fUrl;

        protected WebBasePage(HttpContext context, RequestDelegate next, PageSourceInfo info)
        {
            TkDebug.AssertArgumentNull(context, nameof(context), null);
            TkDebug.AssertArgumentNull(next, nameof(next), null);
            TkDebug.AssertArgumentNull(info, nameof(info), null);

            Context = context;
            Session = context.Session;
            Request = context.Request;
            Response = context.Response;
            Next = next;
            QueryString = new QueryStringWrapper(Request.Query);
            SessionGbl = WebGlobalVariable.SessionGbl;
            IsPost = HttpMethods.IsPost(Request.Method);
            fSupportLogOn = true;

            string localUrl = Request.GetDisplayUrl();
            fUrl = new Uri(localUrl);
            SelfUrl = InternalUriUtil.GetSelfUrl(new Uri(localUrl));
            string retUrl = ObjectUtil.GetDefaultValue(Request.Query["RetUrl"].ToString(), string.Empty);
            RetUrl = string.IsNullOrEmpty(retUrl) ? null : new Uri(retUrl, UriKind.RelativeOrAbsolute);
            fPageSourceInfo = info;
            //Style = fPageSourceInfo.Style;
        }

        #region IWebHandler 成员

        public HttpRequest Request { get; private set; }

        public HttpResponse Response { get; private set; }

        public ISession Session { get; private set; }

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
                return fUrl;
            }
        }

        public string PageExtension
        {
            get
            {
                //return InternalUriUtil.GetUrlExtension(Request.Url);
                return null;
            }
        }

        public bool IsMobileDevice
        {
            get
            {
                return false;
                //return Request.Browser.IsMobileDevice;
            }
        }

        public RequestDelegate Next { get; }

        public HttpContext Context { get; }

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
                return WebUtil.GetQueryString(Request);
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

        private Task Post()
        {
            PreparePostData();
            return DoPost();
        }

        protected void CheckFunctionRight()
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
                        if (Style.Style == PageStyle.List || Style.Style == PageStyle.Detail
                            || MetaDataUtil.Equals(Style, DbListSource.TabStyle))
                        {
                            // List和Detail不禁止子功能权限，子功能权限主要控制Insert，Update这些操作。如果想禁止查看数据，考虑使用DataRight过滤
                            break;
                        }
                        var subKey = GetSubFunctionKey();
                        if (!functionRight.IsSubFunction(subKey, functionSource.FunctionKey))
                            throw new NoFunctionRightException();
                        break;

                    case FunctionRightType.None:
                        break;
                }
            }
        }

        private SubFunctionKey GetSubFunctionKey()
        {
            return new SubFunctionKey(Style, fPageSourceInfo.Source);
        }

        protected virtual void PreparePostData()
        {
            MemoryStream stream = new MemoryStream();
            byte[] data = null;
            try
            {
                IPostObjectCreator creator = PostObjectCreator;
                if (creator == null)
                    PostObject = null;
                else
                {
                    //FileUtil.CopyStream(Request.Body, stream);
                    Request.Body.CopyToAsync(stream).GetAwaiter().GetResult();
                    if (BaseAppSetting.Current.IsDebug)
                    {
                        data = stream.ToArray();
                        stream.Position = 0;
                    }
                    PostObject = creator.Read(this, stream);
                }
            }
            catch (Exception ex)
            {
                TkDebug.ThrowIfNoAppSetting();
                if (BaseAppSetting.Current.IsDebug)
                {
                    string file = Path.Combine(BaseAppSetting.Current.XmlPath, "Post.txt");
                    string content = Encoding.UTF8.GetString(data);

                    FileUtil.WriteFileUseWorkThread(file, content);
                }
                TkDebug.ThrowToolkitException("提交上来的数据不是XML，如果Debug开启，可以查看Post.txt中的内容", ex, this);
            }
            finally
            {
                stream.Dispose();
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

        protected abstract Task DoGet();

        protected abstract Task DoPost();

        protected virtual Task HandleException(Exception ex)
        {
            return null;
        }

        internal Task LoadPage()
        {
            if (SupportLogOn)
                SessionGbl.AppRight.LogOnRight.CheckLogOn(SessionGbl.Info.UserId, SelfUrl);

            try
            {
                if (IsPost)
                    return Post();
                else
                    return DoGet();
            }
            catch (Exception ex)
            {
                Task result = HandleException(ex);
                if (result == null)
                    throw new Exception(ex.Message, ex);
                return result;
            }
        }
    }
}