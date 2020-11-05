using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Web;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.AliyunOSS
{
    internal class AliyunOSSUploadInfo : UploadInfo
    {
        public AliyunOSSUploadInfo(IFormFile file)
        {
            TkDebug.ThrowIfNoAppSetting();

            FileSize = (int)file.Length;
            ContentType = file.ContentType;
            FileName = Path.GetFileName(file.FileName);
            WebAppSetting appsetting = WebAppSetting.WebCurrent;
            string newFileName = Guid.NewGuid().ToString();
            TempFile = new FileConfig(AliyunOSSSetting.Current.TempBucketName, newFileName);
            ServerPath = TempFile.ToString();
            WebPath = WebUtil.ResolveUrl(TempFile.AccessUrl);
        }

        public FileConfig TempFile { get; private set; }
    }
}