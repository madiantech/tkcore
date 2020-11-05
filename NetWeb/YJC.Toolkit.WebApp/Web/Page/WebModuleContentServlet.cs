namespace YJC.Toolkit.Web.Page
{
    public class WebModuleContentServlet : ToolkitServlet
    {
        protected override WebBasePage CreatePage()
        {
            return new WebModuleContentPage();
        }
    }
}
