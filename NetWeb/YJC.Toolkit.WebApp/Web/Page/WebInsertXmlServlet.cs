namespace YJC.Toolkit.Web.Page
{
    public class WebInsertXmlServlet : ToolkitServlet
    {
        protected override WebBasePage CreatePage()
        {
            return new WebInsertXmlPage();
        }
    }
}
