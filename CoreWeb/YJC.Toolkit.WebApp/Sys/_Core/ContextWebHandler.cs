using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Sys
{
    internal class ContextWebHandler : IWebHandler
    {
        private readonly HttpContext fContext;
        private readonly PageSourceInfo fInfo;
        private readonly IPageStyle fStyle;
        private ICallerInfo fCallerInfo;

        public ContextWebHandler(HttpContext context, PageSourceInfo info)
            : this(context, info, info.Style)
        {
        }

        public ContextWebHandler(HttpContext context, PageSourceInfo info, IPageStyle style)
        {
            fContext = context;
            fInfo = info;
            fStyle = style;
        }

        public HttpRequest Request => fContext.Request;

        public HttpResponse Response => fContext.Response;

        public ISession Session => fContext.Session;

        public string Title => string.Empty;

        public Uri PageUrl => new Uri(Request.GetDisplayUrl());

        public string PageExtension => string.Empty;

        public bool IsMobileDevice => false;

        public IPageStyle Style => fStyle;

        public bool IsPost => HttpMethods.IsPost(Request.Method);

        public IQueryString QueryString => new YJC.Toolkit.Web.QueryStringWrapper(Request.Query);

        public object PostObject => null;

        public ICallerInfo CallerInfo
        {
            get
            {
                if (fCallerInfo == null)
                    fCallerInfo = new InternalCallerInfo(this, WebGlobalVariable.SessionGbl, null, null);

                return fCallerInfo;
            }
        }

        public string QueryStringText => WebUtil.GetQueryString(Request);

        public PageSourceInfo SourceInfo => fInfo;
    }
}