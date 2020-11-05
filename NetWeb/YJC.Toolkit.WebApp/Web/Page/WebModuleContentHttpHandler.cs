using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web.Page
{
    [RegClass(Author = "YJC", CreateDate = "2013-10-12",
        Description = "Module输出内容页面的HttpHandler对象")]
    internal sealed class WebModuleContentHttpHandler : ToolkitHttpHandler
    {
        protected override WebBasePage CreatePage()
        {
            return new WebModuleContentPage();
        }
    }
}
