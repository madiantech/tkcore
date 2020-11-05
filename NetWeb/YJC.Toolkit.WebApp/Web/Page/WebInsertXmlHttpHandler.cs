using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web.Page
{
    [RegClass(Author = "YJC", CreateDate = "2013-10-14",
        Description = "Insert页面的HttpHandler对象")]
    internal sealed class WebInsertXmlHttpHandler : ToolkitHttpHandler
    {
        protected override WebBasePage CreatePage()
        {
            return new WebInsertXmlPage();
        }
    }
}
