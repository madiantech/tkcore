using System.IO;
using System.Text;
using System.Web;
using Aliyun.OSS;
using Microsoft.AspNetCore.Http;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.AliyunOSS
{
    public static class AliyunOSSUtil
    {
        internal static string GetAccessUrl(FileConfig config)
        {
            string base64 = config.ToBase64();
            string url = string.Format("~/c/~source/C/AliyunOSSUrl?File={0}", HttpUtility.UrlEncode(base64));
            return url;
        }

        private static string GetDisposition(string fileName)
        {
            return string.Format(ObjectUtil.SysCulture, "attachment;filename*=utf-8''{0}",
                HttpUtility.UrlEncode(fileName, Encoding.UTF8));
        }

        public static ObjectMetadata UseOriginName(string fileName)
        {
            TkDebug.AssertArgumentNullOrEmpty(fileName, "fileName", null);

            ObjectMetadata metaData = new ObjectMetadata
            {
                ContentDisposition = GetDisposition(fileName)
            };

            return metaData;
        }

        public static ObjectMetadata CreateMetaData(IFormFile file)
        {
            TkDebug.AssertArgumentNull(file, "file", null);

            string fileName = Path.GetFileName(file.FileName);
            ObjectMetadata metaData = new ObjectMetadata
            {
                ContentType = file.ContentType,
                ContentLength = (int)file.Length,
                ContentDisposition = GetDisposition(fileName)
            };
            return metaData;
        }

        public static bool CreateBucket(string bucketName)
        {
            return CreateBucket(AliyunOSSSetting.CreateClient(), bucketName);
        }

        public static bool CreateBucket(OssClient client, string bucketName)
        {
            TkDebug.AssertArgumentNull(client, "client", null);
            TkDebug.AssertArgumentNullOrEmpty(bucketName, "bucketName", null);

            bucketName = bucketName.ToLower(ObjectUtil.SysCulture);
            if (!client.DoesBucketExist(bucketName))
            {
                client.CreateBucket(bucketName);
                return true;
            }
            return false;
        }
    }
}