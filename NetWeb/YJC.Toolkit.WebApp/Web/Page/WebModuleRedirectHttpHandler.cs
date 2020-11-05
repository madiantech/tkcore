using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web.Page
{
    [RegClass(Author = "YJC", CreateDate = "2013-10-12",
        Description = "Module重定向页面的HttpHandler对象")]
    internal sealed class WebModuleRedirectHttpHandler : ToolkitHttpHandler
    {
        protected override WebBasePage CreatePage()
        {
            return new WebModuleRedirectPage();
        }
    }
}
