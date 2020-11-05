using System;
using System.IO;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [UploadProcessorConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-11-05",
        Author = "YJC", Description = "将上传文件放在指定目录，通过文件方式访问")]
    internal class FileUploadProcessorConfig : IConfigCreator<IUploadProcessor>, IConfigCreator<IUploadProcessor2>
    {
        #region IConfigCreator<IUploadProcessor> 成员

        public IUploadProcessor CreateObject(params object[] args)
        {
            return new FileUploadProcessor(this);
        }

        #endregion IConfigCreator<IUploadProcessor> 成员

        #region IConfigCreator<IUploadProcessor2> 成员

        IUploadProcessor2 IConfigCreator<IUploadProcessor2>.CreateObject(params object[] args)
        {
            return new FileUploadProcessor(this);
        }

        #endregion IConfigCreator<IUploadProcessor2> 成员

        [SimpleAttribute(Required = true)]
        public string BaseUploadPath { get; private set; }

        [SimpleAttribute]
        public string UploadPath { get; private set; }

        [SimpleAttribute(Required = true)]
        public string BaseVirtualPath { get; private set; }

        [SimpleAttribute]
        public string VirtualPath { get; private set; }

        public string CreateVirtualPath()
        {
            Uri baseUri = BaseAppSetting.Current.VerfiyGetHost(BaseVirtualPath);
            if (string.IsNullOrEmpty(VirtualPath))
                return baseUri.ToString();
            return UriUtil.CombineUri(baseUri, VirtualPath).ToString();
        }

        public string CreateUploadPath()
        {
            string path = BaseAppSetting.Current.VerfiyGetHostString(BaseUploadPath);
            if (string.IsNullOrEmpty(UploadPath))
                return path;
            return Path.Combine(path, UploadPath);
        }
    }
}