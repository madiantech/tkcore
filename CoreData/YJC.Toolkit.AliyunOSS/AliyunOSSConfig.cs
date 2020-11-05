using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.AliyunOSS
{
    internal class AliyunOSSConfig
    {
        [SimpleAttribute(DefaultValue = "1:00:00")]
        public TimeSpan UrlCacheTime { get; private set; }

        [SimpleAttribute(Required = true)]
        public string DefaultBucketName { get; private set; }

        [SimpleAttribute(Required = true)]
        public string TempBucketName { get; private set; }

        [SimpleAttribute]
        public bool UseAliyunUploadMode { get; private set; }

        [DynamicElement(AliyunOSSConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit, Required = true)]
        public IConfigCreator<ClientConfig> Config { get; private set; }
    }
}