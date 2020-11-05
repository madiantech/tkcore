using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web.Page
{
    public class WebInitServlet : ToolkitServlet
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
