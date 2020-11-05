using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web.Page
{
    [RegClass(Author = "YJC", CreateDate = "2013-10-14",
        Description = "Delete页面的HttpHandler对象")]
    internal sealed class WebDeleteXmlHttpHandler : ToolkitHttpHandler
    {
        protected override WebBasePage CreatePage()
        {
            if (Request.QueryString["Content"] == "true")
                return new WebDeleteContentXmlPage();
            return new WebDeleteXmlPage();
        }
    }
}
