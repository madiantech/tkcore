using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.AliyunOSS
{
    [UploadProcessorConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2019-01-15",
        Author = "YJC", Description = "将文件上传的阿里云OSS服务中")]
    internal class AliyunOSSUploadProcessorConfig : IConfigCreator<IUploadProcessor>,
        IConfigCreator<IUploadProcessor2>
    {
        #region IConfigCreator<IUploadProcessor> 成员

        public IUploadProcessor CreateObject(params object[] args)
        {
            return CreateProcessor();
        }

        #endregion IConfigCreator<IUploadProcessor> 成员

        #region IConfigCreator<IUploadProcessor2> 成员

        IUploadProcessor2 IConfigCreator<IUploadProcessor2>.CreateObject(params object[] args)
        {
            return CreateProcessor();
        }

        #endregion IConfigCreator<IUploadProcessor2> 成员

        [SimpleAttribute]
        public string BucketName { get; private set; }

        [SimpleAttribute]
        public bool? UseAliyunUploadMode { get; private set; }

        private bool CalcUseAliyunUploadMode()
        {
            if (UseAliyunUploadMode == null)
                return AliyunOSSSetting.Current.UseAliyunUploadMode;
            return UseAliyunUploadMode.Value;
        }

        private AliyunOSSUploadProcessor CreateProcessor()
        {
            return new AliyunOSSUploadProcessor(BucketName)
            {
                UseAliyunUploadMode = CalcUseAliyunUploadMode()
            };
        }
    }
}