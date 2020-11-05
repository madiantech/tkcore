using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class UploadFileInfo
    {
        public UploadFileInfo(IFormFile file)
        {
            FileName = file.FileName;
            ContentType = file.ContentType;
            Length = (int)file.Length;
            ServerPath = Path.Combine(WebAppSetting.WebCurrent.UploadTempPath,
                Guid.NewGuid() + Path.GetExtension(FileName));
            //file.SaveAs(ServerPath);
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