using System;
using Aliyun.OSS;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.AliyunOSS
{
    public class AliyunOSSSetting
    {
        private static AliyunOSSSetting fCurrent;

        private AliyunOSSSetting(AliyunOSSConfigXml xml)
        {
            ClientConfig = xml.AliyunOSS.Config.CreateObject();
            UrlCacheTime = xml.AliyunOSS.UrlCacheTime;
            DefaultBucketName = xml.AliyunOSS.DefaultBucketName.ToLower(ObjectUtil.SysCulture);
            TempBucketName = xml.AliyunOSS.TempBucketName.ToLower(ObjectUtil.SysCulture);
            UseAliyunUploadMode = xml.AliyunOSS.UseAliyunUploadMode;
        }

        public ClientConfig ClientConfig { get; private set; }

        public string DefaultBucketName { get; private set; }

        public string TempBucketName { get; private set; }

        public TimeSpan UrlCacheTime { get; private set; }

        public bool UseAliyunUploadMode { get; private set; }

        public static AliyunOSSSetting Current
        {
            get
            {
                TkDebug.AssertNotNull(fCurrent, "阿里云OSS没有被初始化，请确认是否存在AliyunOSS.xml", null);
                return fCurrent;
            }
        }

        internal static AliyunOSSSetting CreateAliyunOSSSetting(AliyunOSSConfigXml xml)
        {
            fCurrent = new AliyunOSSSetting(xml);
            return fCurrent;
        }

        public static OssClient CreateClient()
        {
            return Current.ClientConfig.CreateClient();
        }
    }
}