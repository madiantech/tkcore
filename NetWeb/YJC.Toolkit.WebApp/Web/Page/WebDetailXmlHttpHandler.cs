using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web.Page
{
    [RegClass(Author = "YJC", CreateDate = "2013-10-14",
        Description = "Detail页面的HttpHandler对象")]
    internal sealed class WebDetailXmlHttpHandler : ToolkitHttpHandler
    {
        protected override WebBasePage CreatePage()
        {
            return new WebDetailXmlPage();
        }
    }
}
