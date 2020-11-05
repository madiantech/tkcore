using Microsoft.AspNetCore.Http;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web.Page
{
    public class WebModuleContentPage : WebBaseModuleContentPage
    {
        private readonly IModule fModule;

        public WebModuleContentPage(HttpContext context, RequestDelegate next, PageSourceInfo info)
            : base(context, next, info)
        {
            fModule = UrlInfo.CreateModule();
        }

        protected override IModule Module
        {
            get
            {
                return fModule;
            }
        }

        protected override bool SupportLogOn
        {
            get
            {
                return fModule.IsSupportLogOn(this);
            }
            set
            {
                base.SupportLogOn = value;
            }
        }
    }
}