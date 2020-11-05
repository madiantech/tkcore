namespace YJC.Toolkit.Web.Page
{
    public class WebDeleteXmlServlet : ToolkitServlet
    {
        protected override WebBasePage CreatePage()
        {
            if (Request.QueryString["Content"] == "true")
                return new WebDeleteContentXmlPage();
            return new WebDeleteXmlPage();
        }
    }
}
