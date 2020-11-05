using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    class WebUploadResult : WebSuccessResult
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
