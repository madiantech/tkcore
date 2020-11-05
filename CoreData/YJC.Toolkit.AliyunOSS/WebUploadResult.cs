using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.AliyunOSS
{
    internal class WebUploadResult : WebSuccessResult
    {
        public WebUploadResult(UploadInfo uploadInfo)
            : base(string.Empty)
        {
            UploadInfo = uploadInfo;
        }

        [ObjectElement]
        public UploadInfo UploadInfo { get; private set; }
    }
}