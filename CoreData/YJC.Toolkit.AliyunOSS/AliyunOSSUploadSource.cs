using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Web;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.AliyunOSS
{
    [Source(Author = "YJC", CreateDate = "2019-01-16",
        Description = "将文件临时保存在阿里云上")]
    [JsonObjectPageMaker, NoPostObjectCreator]
    [WebPage(SupportLogOn = false)]
    internal class AliyunOSSUploadSource : ISource
    {
        public AliyunOSSUploadSource()
        {
        }

        #region ISource 成员

        public OutputData DoAction(IInputData input)
        {
            HttpRequest request = WebGlobalVariable.Request;
            IFormFileCollection fileCollection = request.ReadFormAsync().GetAwaiter().GetResult().Files;
            IFormFile file = fileCollection["Filedata"];
            if (file?.Length > 0 && !string.IsNullOrEmpty(file?.FileName))
            {
                AliyunOSSUploadInfo upload = new AliyunOSSUploadInfo(file);
                try
                {
                    using (MemoryStream stream = new MemoryStream(upload.FileSize))
                    {
                        file.CopyTo(stream);
                        var metaData = AliyunOSSUtil.CreateMetaData(file);
                        stream.Flush();
                        stream.Position = 0;
                        upload.TempFile.UploadFile(stream, metaData);
                        return OutputData.CreateToolkitObject(new WebUploadResult(upload));
                    }
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