using System;
using System.IO;
using System.Web;
using Microsoft.AspNetCore.Http;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class UploadSource : ISource
    {
        #region ISource 成员

        public OutputData DoAction(IInputData input)
        {
            HttpRequest request = WebGlobalVariable.Request;
            IFormFileCollection fileCollection = request.ReadFormAsync().GetAwaiter().GetResult().Files;
            IFormFile file = fileCollection["Filedata"];
            if (file?.Length > 0 && !string.IsNullOrEmpty(file.FileName))
            {
                UploadInfo upload = new WebUploadInfo(file);
                try
                {
                    using (FileStream stream = new FileStream(upload.ServerPath, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        file.CopyTo(stream);
                    }
                    return OutputData.CreateToolkitObject(new WebUploadResult(upload));
                }
                catch (Exception ex)
                {
                    return OutputData.CreateToolkitObject(new WebErrorResult(ex.Message));
                }
            }
            return OutputData.CreateToolkitObject(new WebErrorResult("没有上传文件"));
        }

        #endregion ISource 成员
    }
}