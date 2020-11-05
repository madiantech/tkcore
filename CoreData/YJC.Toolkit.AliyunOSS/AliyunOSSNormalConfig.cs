using YJC.Toolkit.Sys;

namespace YJC.Toolkit.AliyunOSS
{
    [AliyunOSSConfig(NamespaceType = NamespaceType.Toolkit, RegName = "Normal", Author = "YJC",
        CreateDate = "2019-01-10", Description = "常规阿里云OSS参数定义")]
    internal class AliyunOSSNormalConfig : IConfigCreator<ClientConfig>
    {
        #region IConfigCreator<ClientConfig> 成员

        public ClientConfig CreateObject(params object[] args)
        {
            return new ClientConfig(EndPoint, AccessKeyId, AccessKeySecret);
        }

        #endregion IConfigCreator<ClientConfig> 成员

        [SimpleAttribute(DefaultValue = AliyunOSSConst.DEFAULT_END_POINT)]
        public string EndPoint { get; private set; }

        [SimpleAttribute(Required = true)]
        public string AccessKeyId { get; private set; }

        [SimpleAttribute(Required = true)]
        public string AccessKeySecret { get; private set; }
    }
}