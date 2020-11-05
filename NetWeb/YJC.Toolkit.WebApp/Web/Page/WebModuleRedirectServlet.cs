namespace YJC.Toolkit.Web.Page
{
    public class WebModuleRedirectServlet : ToolkitServlet
    {
        protected override WebBasePage CreatePage()
        {
            return new WebModuleRedirectPage();
        }
    }
}
