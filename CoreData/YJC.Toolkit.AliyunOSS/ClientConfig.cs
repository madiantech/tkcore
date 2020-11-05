using Aliyun.OSS;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.AliyunOSS
{
    public class ClientConfig
    {
        public ClientConfig(string endPoint, string accessKeyId, string accessKeySecret)
        {
            EndPoint = endPoint;
            AccessKeyId = accessKeyId;
            AccessKeySecret = accessKeySecret;
        }

        [SimpleAttribute]
        public string EndPoint { get; private set; }

        [SimpleAttribute]
        public string AccessKeyId { get; private set; }

        [SimpleAttribute]
        public string AccessKeySecret { get; private set; }

        public OssClient CreateClient()
        {
            return new OssClient(EndPoint, AccessKeyId, AccessKeySecret);
        }
    }
}