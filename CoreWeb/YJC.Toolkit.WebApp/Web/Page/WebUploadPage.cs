using System;
using Microsoft.AspNetCore.Http;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web.Page
{
    [WebPagePlugIn(Author = "YJC", CreateDate = "2014-06-21",
        Description = "上传文件的处理页面")]
    internal class WebUploadPage : WebBaseContentPage
    {
        private readonly ISource fSource;

        public WebUploadPage(HttpContext context, RequestDelegate next, PageSourceInfo info)
            : base(context, next, info)
        {
            SupportLogOn = false;
            fSource = new UploadSource();
        }

        protected override IPageMaker PageMaker
        {
            get
            {
                return new JsonObjectPageMaker();
            }
        }

        protected override IPostObjectCreator PostObjectCreator
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public override ISource Source
        {
            get
            {
                return fSource;
            }
        }

        protected override void PreparePostData()
        {
        }
    }
}