using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public class WebStartPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TkDebug.ThrowIfNoAppSetting();

            var current = WebAppSetting.WebCurrent;
            //current.StartUrl = UriUtil.GetBaseUri(Request.Url);
            string url = current.StartupPath;
            if (!string.IsNullOrEmpty(url))
                Response.Redirect(url, false);
            else
            {
                Response.Clear();
                Response.StatusCode = 404;
                Response.Status = "404 Page Not Found";
                Response.End();
            }
        }
    }
}
