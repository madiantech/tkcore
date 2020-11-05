using System;
using System.IO;
using System.Text;
using Aliyun.OSS;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.AliyunOSS
{
    public class FileConfig : IEquatable<FileConfig>
    {
        private FileConfig()
        {
        }

        public FileConfig(string objectName)
            : this(null, objectName)
        {
        }

        public FileConfig(string bucketName, string objectName)
        {
            TkDebug.AssertArgumentNullOrEmpty(objectName, "objectName", null);

            if (string.IsNullOrEmpty(bucketName))
                BucketName = AliyunOSSSetting.Current.DefaultBucketName;
            else
                BucketName = bucketName.ToLower();
            ObjectName = objectName;
        }

        #region IEquatable<FileConfig> 成员

        public bool Equals(FileConfig other)
        {
            if (other == null)
                return false;

            return BucketName == other.BucketName && ObjectName == other.ObjectName;
        }

        #endregion IEquatable<FileConfig> 成员

        [SimpleAttribute]
        public string BucketName { get; private set; }

        [SimpleAttribute]
        public string ObjectName { get; private set; }

        public string AccessUrl
        {
            get
            {
                return AliyunOSSUtil.GetAccessUrl(this);
            }
        }

        public override int GetHashCode()
        {
            return BucketName.GetHashCode() + ObjectName.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            FileConfig other = obj as FileConfig;
            if (other == null)
                return false;
            return Equals(other);
        }

        public override string ToString()
        {
            return this.WriteJson();
        }

        public string ToBase64()
        {
            string data = ToString();
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            return Convert.ToBase64String(byteData);
        }

        public void UploadFile(string fileName, string originFileName)
        {
            TkDebug.AssertArgumentNullOrEmpty(originFileName, "originFileName", this);

            ObjectMetadata metaData = AliyunOSSUtil.UseOriginName(originFileName);
            UploadFile(AliyunOSSSetting.CreateClient(), fileName, metaData);
        }

        public void UploadFile(string fileName)
        {
            UploadFile(AliyunOSSSetting.CreateClient(), fileName);
        }

        public void UploadFile(string fileName, ObjectMetadata metaData)
        {
            UploadFile(AliyunOSSSetting.CreateClient(), fileName, metaData);
        }

        public void UploadFile(Stream stream)
        {
            UploadFile(AliyunOSSSetting.CreateClient(), stream);
        }

        public void UploadFile(Stream stream, ObjectMetadata metaData)
        {
            UploadFile(AliyunOSSSetting.CreateClient(), stream, metaData);
        }

        public void UploadFile(OssClient client, string fileName)
        {
            TkDebug.AssertArgumentNull(client, "client", this);
            TkDebug.AssertArgumentNullOrEmpty(fileName, "fileName", this);
            TkDebug.AssertArgumentNull(File.Exists(fileName),
                string.Format(ObjectUtil.SysCulture, "{0}不存在，请确认", fileName), this);

            client.PutObject(BucketName, ObjectName, fileName);
        }

        public void UploadFile(OssClient client, string fileName, ObjectMetadata metaData)
        {
            TkDebug.AssertArgumentNull(client, "client", this);
            TkDebug.AssertArgumentNullOrEmpty(fileName, "fileName", this);
            TkDebug.AssertArgumentNull(metaData, "metaData", this);
            TkDebug.AssertArgumentNull(File.Exists(fileName),
                string.Format(ObjectUtil.SysCulture, "{0}不存在，请确认", fileName), this);

            client.PutObject(BucketName, ObjectName, fileName, metaData);
        }

        public void UploadFile(OssClient client, Stream stream)
        {
            TkDebug.AssertArgumentNull(client, "client", this);
            TkDebug.AssertArgumentNull(stream, "stream", this);

            client.PutObject(BucketName, ObjectName, stream);
        }

        public void UploadFile(OssClient client, Stream stream, ObjectMetadata metaData)
        {
            TkDebug.AssertArgumentNull(client, "client", this);
            TkDebug.AssertArgumentNull(stream, "stream", this);
            TkDebug.AssertArgumentNull(metaData, "metaData", this);

            client.PutObject(BucketName, ObjectName, stream, metaData);
        }

        public void Download(Stream downloadStream)
        {
            Download(AliyunOSSSetting.CreateClient(), downloadStream);
        }

        public void Download(OssClient client, Stream downloadStream)
        {
            TkDebug.AssertArgumentNull(client, "client", this);
            TkDebug.AssertArgumentNull(downloadStream, "downloadStream", this);

            var obj = client.GetObject(BucketName, ObjectName);
            using (obj)
            {
                FileUtil.CopyStream(obj.Content, downloadStream);
            }
        }

        public Uri GenerateUri()
        {
            return GenerateUri(AliyunOSSSetting.CreateClient());
        }

        public Uri GenerateUri(OssClient client)
        {
            TkDebug.AssertArgumentNull(client, "client", this);

            return client.GeneratePresignedUri(BucketName, ObjectName,
                DateTime.Now + AliyunOSSSetting.Current.UrlCacheTime);
        }

        public void Delete()
        {
            Delete(AliyunOSSSetting.CreateClient());
        }

        public void Delete(OssClient client)
        {
            TkDebug.AssertArgumentNull(client, "client", this);

            client.DeleteObject(BucketName, ObjectName);
        }

        public void CopyTo(FileConfig target, ObjectMetadata metaData = null)
        {
            CopyTo(AliyunOSSSetting.CreateClient(), target, metaData);
        }

        public void CopyTo(OssClient client, FileConfig target, ObjectMetadata metaData = null)
        {
            TkDebug.AssertArgumentNull(client, "client", this);
            TkDebug.AssertArgumentNull(target, "target", this);

            var req = new CopyObjectRequest(BucketName, ObjectName, target.BucketName, target.ObjectName);
            if (metaData != null)
                req.NewObjectMetadata = metaData;
            client.CopyObject(req);
        }

        public void RemoveTo(FileConfig target, ObjectMetadata metaData = null)
        {
            RemoveTo(AliyunOSSSetting.CreateClient(), target, metaData);
        }

        public void RemoveTo(OssClient client, FileConfig target, ObjectMetadata metaData = null)
        {
            TkDebug.AssertArgumentNull(client, "client", this);
            TkDebug.AssertArgumentNull(target, "target", this);

            CopyTo(client, target, metaData);
            Delete(client);
        }

        public bool Exists()
        {
            return Exists(AliyunOSSSetting.CreateClient());
        }

        public bool Exists(OssClient client)
        {
            TkDebug.AssertArgumentNull(client, "client", this);

            return client.DoesObjectExist(BucketName, ObjectName);
        }

        public static FileConfig ReadFromJson(string json)
        {
            TkDebug.AssertArgumentNullOrEmpty(json, "json", null);

            FileConfig config = new FileConfig();
            config.ReadJson(json);
            return config;
        }

        public static FileConfig ReadFromBase64(string base64Str)
        {
            TkDebug.AssertArgumentNullOrEmpty(base64Str, "base64Str", null);

            byte[] data = Convert.FromBase64String(base64Str);
            string json = Encoding.UTF8.GetString(data);
            return ReadFromJson(json);
        }
    }
}