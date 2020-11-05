using System;
using System.Web;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    class UploadSource : ISource
    {
        #region ISource 成员

        public OutputData DoAction(IInputData input)
        {
            HttpRequest request = WebGlobalVariable.Request;
            HttpPostedFile file = request.Files == null ? null : request.Files["Filedata"];
            if (file != null && file.ContentLength > 0 && !string.IsNullOrEmpty(file.FileName))
            {
                UploadInfo upload = new WebUploadInfo(file);
                try
                {
                    file.SaveAs(upload.ServerPath);
                    return OutputData.CreateToolkitObject(new WebUploadResult(upload));
                }
                catch (Exception ex)
                {
                    return OutputData.CreateToolkitObject(new WebErrorResult(ex.Message));
                }
            }
            return OutputData.CreateToolkitObject(new WebErrorResult("没有上传文件"));
        }

        #endregion
    }
}
