using System;
using System.Text;
using System.Web;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public class ToolkitUploadPage : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Load += Page_Load;
        }

        private void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod == "POST" && Request.Files.Count > 0)
            {
                HttpPostedFile file = Request.Files[0];
                UploadFileInfo fileInfo = new UploadFileInfo(file);
                string output = fileInfo.WriteJson();
                Response.ContentType = ContentTypeConst.JSON;
                Response.ContentEncoding = Encoding.UTF8;
                Response.Write(output);
            }
        }
    }
}
