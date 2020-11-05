using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class WebUploadInfo : UploadInfo
    {
        public WebUploadInfo(IFormFile file)
        {
            TkDebug.ThrowIfNoAppSetting();

            FileSize = (int)file.Length;
            ContentType = file.ContentType;
            FileName = Path.GetFileName(file.FileName);
            WebAppSetting appsetting = WebAppSetting.WebCurrent;
            string newFileName = Guid.NewGuid() + Path.GetExtension(FileName);
            ServerPath = Path.Combine(appsetting.UploadTempPath, newFileName);
            string urlPath = UriUtil.TextCombine(appsetting.UploadTempVirtualPath, newFileName);
            WebPath = AppUtil.ResolveUrl(urlPath);
        }
    }
}