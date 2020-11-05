using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web.Page
{
    [RegClass(Author = "YJC", CreateDate = "2014-06-21",
       Description = "Init WebPage页面的HttpHandler对象")]
    internal class WebInitHttpHandler : ToolkitHttpHandler
    {
        protected override WebBasePage CreatePage()
        {
            string source = Request.QueryString["Source"];
            TkDebug.AssertNotNullOrEmpty(source, "QueryString的Source变量值为空", this);
            return PlugInFactoryManager.CreateInstance<WebBasePage>(
                WebPagePlugInFactory.REG_NAME, source);
        }
    }
}
