using System;
using System.IO;
using System.Web;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    class UploadFileInfo
    {
        public UploadFileInfo(HttpPostedFile file)
        {
            FileName = file.FileName;
            ContentType = file.ContentType;
            Length = file.ContentLength;
            ServerPath = Path.Combine(WebAppSetting.WebCurrent.UploadTempPath,
                Guid.NewGuid() + Path.GetExtension(FileName));
            file.SaveAs(ServerPath);
        }

        [SimpleAttribute]
        public string FileName { get; private set; }

        [SimpleAttribute]
        public string ServerPath { get; private set; }

        [SimpleAttribute]
        public int Length { get; private set; }

        [SimpleAttribute]
        public string ContentType { get; private set; }
    }
}
