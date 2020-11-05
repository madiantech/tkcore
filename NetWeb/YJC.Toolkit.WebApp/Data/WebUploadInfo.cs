using System;
using System.IO;
using System.Web;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class WebUploadInfo : UploadInfo
    {
        public WebUploadInfo(HttpPostedFile file)
        {
            TkDebug.ThrowIfNoAppSetting();

            FileSize = file.ContentLength;
            ContentType = file.ContentType;
            FileName = Path.GetFileName(file.FileName);
            WebAppSetting appsetting = WebAppSetting.WebCurrent;
            string newFileName = Guid.NewGuid() + Path.GetExtension(FileName);
            ServerPath = Path.Combine(appsetting.UploadTempPath, newFileName);
            WebPath = VirtualPathUtility.Combine(appsetting.UploadTempVirtualPath, newFileName);
            WebPath = WebUtil.ResolveUrl(WebPath);
        }
    }
}
