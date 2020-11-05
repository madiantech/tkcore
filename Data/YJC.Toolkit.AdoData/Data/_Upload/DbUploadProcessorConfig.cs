using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [UploadProcessorConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-06-23",
        Author = "YJC", Description = "将上传文件存入SYS_ATTACHMENT表")]
    class DbUploadProcessorConfig : IConfigCreator<IUploadProcessor>, IConfigCreator<IUploadProcessor2>
    {
        #region IConfigCreator<IUploadProcessor> 成员

        public IUploadProcessor CreateObject(params object[] args)
        {
            return new DbUploadProcessor(this);
        }

        #endregion

        #region IConfigCreator<IUploadProcessor2> 成员

        IUploadProcessor2 IConfigCreator<IUploadProcessor2>.CreateObject(params object[] args)
        {
            return new DbUploadProcessor(this);
        }

        #endregion

        [SimpleAttribute(DefaultValue = true)]
        public bool OutputFileName { get; private set; }

        [SimpleAttribute(DefaultValue = true)]
        public bool CacheFile { get; private set; }

    }
}
