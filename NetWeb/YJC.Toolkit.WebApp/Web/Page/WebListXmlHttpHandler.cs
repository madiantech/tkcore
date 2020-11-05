using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web.Page
{
    [RegClass(Author = "YJC", CreateDate = "2013-10-14",
        Description = "List页面的HttpHandler对象")]
    internal sealed class WebListXmlHttpHandler : ToolkitHttpHandler
    {
        protected override WebBasePage CreatePage()
        {
            return new WebListXmlPage();
        }
    }
}
