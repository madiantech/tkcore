using Microsoft.AspNetCore.Http;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web.Page;

namespace YJC.Toolkit.Web
{
    internal class WebExcelPage : WebModuleContentPage
    {
        public WebExcelPage(HttpContext context, RequestDelegate next, PageSourceInfo info)
            : base(context, next, info)
        {
        }

        protected override IPageMaker PageMaker
        {
            get
            {
                if (IsTemplate)
                    return new ExportExcelHeaderPageMaker();
                else
                    return new ExportExcelPageMaker();
            }
        }

        public override ISource Source
        {
            get
            {
                return IsTemplate ? new EmptySource(true) : base.Source;
            }
        }

        public bool IsTemplate
        {
            get
            {
                return Request.Query["Excel"] == "template";
            }
        }
    }
}