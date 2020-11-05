namespace YJC.Toolkit.Web.Page
{
    public class WebUpdateXmlServlet : ToolkitServlet
    {
        protected override WebBasePage CreatePage()
        {
            return new WebUpdateXmlPage();
        }
    }
}
