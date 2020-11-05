namespace YJC.Toolkit.Web.Page
{
    public class WebListXmlServlet : ToolkitServlet
    {
        protected override WebBasePage CreatePage()
        {
            return new WebListXmlPage();
        }
    }
}
