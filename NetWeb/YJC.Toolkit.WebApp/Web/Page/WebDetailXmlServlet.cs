namespace YJC.Toolkit.Web.Page
{
    public class WebDetailXmlServlet : ToolkitServlet
    {
        protected override WebBasePage CreatePage()
        {
            return new WebDetailXmlPage();
        }
    }
}
