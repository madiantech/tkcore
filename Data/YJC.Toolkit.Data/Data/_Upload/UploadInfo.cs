using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class UploadInfo : IObjectUpload
    {
        public UploadInfo()
        {
        }

        public UploadInfo(IObjectUpload uploadObj)
        {
            TkDebug.AssertArgumentNull(uploadObj, "uploadObj", null);

            FileName = uploadObj.FileName;
            ContentType = uploadObj.ContentType;
            ServerPath = uploadObj.ServerPath;
            FileSize = uploadObj.FileSize;
            WebPath = uploadObj.WebPath;
        }

        [SimpleAttribute]
        public string FileName { get; set; }

        [SimpleAttribute]
        public string ContentType { get; set; }

        [SimpleAttribute]
        public string ServerPath { get; set; }

        [SimpleAttribute]
        public int FileSize { get; set; }

        [SimpleAttribute]
        public string WebPath { get; set; }

        public string ToJson()
        {
            return this.WriteJson();
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(FileName))
                return base.ToString();
            return string.Format(ObjectUtil.SysCulture, "[[{0}, {1}]]", FileName, ContentType);
        }
    }
}
