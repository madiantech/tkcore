using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.AliyunOSS
{
    internal class AliyunOSSUploadProcessor : BaseUploadProcessor, IUploadExtension
    {
        public AliyunOSSUploadProcessor(string bucketName)
        {
            if (string.IsNullOrEmpty(bucketName))
                bucketName = AliyunOSSSetting.Current.DefaultBucketName;

            BucketName = bucketName;
        }

        #region IUploadExtension 成员

        public string UploadUrl
        {
            get
            {
                return UseAliyunUploadMode ? AliyunOSSConst.ALIYUN_UPLOAD_URL : null;
            }
        }

        #endregion IUploadExtension 成员

        public string BucketName { get; private set; }

        public bool UseAliyunUploadMode { get; set; }

        public override UploadInfo CreateValue(IFieldUpload upload, IFieldValueProvider provider)
        {
            TkDebug.AssertArgumentNull(upload, "upload", this);
            TkDebug.AssertArgumentNull(provider, "provider", this);

            string fileName = provider.GetValue(upload.FileNameField);
            if (string.IsNullOrEmpty(fileName))
                return null;

            UploadInfo info = new UploadInfo
            {
                FileName = fileName,
                ContentType = provider.GetValue(upload.MimeTypeField),
                ServerPath = provider.GetValue(upload.ServerPathField),
                FileSize = provider.GetValue<int>(upload.SizeField),
                WebPath = WebUtil.ResolveUrl(provider.GetValue(upload.ServerPathField))
            };
            return info;
        }

        public override UploadInfo CreateValue(IFieldUpload upload, DataRow row)
        {
            TkDebug.AssertArgumentNull(upload, "upload", this);
            TkDebug.AssertArgumentNull(row, "row", this);

            string fileName = row.GetString(upload.FileNameField);
            if (string.IsNullOrEmpty(fileName))
                return null;

            UploadInfo info = new UploadInfo
            {
                FileName = fileName,
                ContentType = row.GetString(upload.MimeTypeField),
                ServerPath = row.GetString(upload.ServerPathField),
                FileSize = row.GetValue<int>(upload.SizeField),
                WebPath = WebUtil.ResolveUrl(row.GetString(upload.ServerPathField))
            };
            return info;
        }

        protected override object DeleteAttachment(IDbDataSource host, IFieldUpload upload,
            IFieldValueAccessor accessor, string path)
        {
            try
            {
                var json = accessor.GetValue(upload.ContentField);
                accessor.SetValue(upload.ContentField, DBNull.Value);
                FileConfig config = FileConfig.ReadFromJson(json);
                config.Delete();
            }
            catch
            {
            }
            return null;
        }

        protected override object DeleteAttachment(IDbDataSource host, IFieldUpload upload,
            DataRow row, string path)
        {
            try
            {
                var json = row[upload.ContentField].ToString();
                row[upload.ContentField] = DBNull.Value;
                FileConfig config = FileConfig.ReadFromJson(json);
                config.Delete();
            }
            catch
            {
            }
            return null;
        }

        public override string Display(IFieldUpload upload, IFieldValueProvider provider)
        {
            string url = provider.GetValue(upload.ServerPathField);
            return WebUtil.ResolveUrl(url);
        }

        public override string Display(IFieldUpload upload, DataRow row)
        {
            string url = row[upload.ServerPathField].ToString();
            return WebUtil.ResolveUrl(url);
        }

        public override IEnumerable<string> GetListSelectFields(TkDbContext context,
            IFieldUpload upload, IFieldInfoIndexer indexer)
        {
            return GetListSelectFields(context, upload, indexer, true, true, true, true);
        }

        protected override object InsertAttachment(IDbDataSource host, IFieldUpload upload,
            IFieldValueAccessor accessor, string path)
        {
            if (UseAliyunUploadMode)
            {
                FileConfig temp = FileConfig.ReadFromJson(path);
                FileConfig config = new FileConfig(BucketName, temp.ObjectName);
                accessor.SetValue(upload.ServerPathField, config.AccessUrl);
                accessor.SetValue(upload.ContentField, config.ToString());

                temp.RemoveTo(config);
            }
            else
            {
                string fileName = Path.GetFileNameWithoutExtension(path);
                FileConfig config = new FileConfig(BucketName, fileName);
                accessor.SetValue(upload.ServerPathField, config.AccessUrl);
                accessor.SetValue(upload.ContentField, config.ToString());
                string originFileName = accessor.GetValue(upload.FileNameField);

                config.UploadFile(path, originFileName);
            }
            return null;
        }

        protected override object InsertAttachment(IDbDataSource host, IFieldUpload upload,
            DataRow row, string path)
        {
            if (UseAliyunUploadMode)
            {
                FileConfig temp = FileConfig.ReadFromJson(path);
                FileConfig config = new FileConfig(BucketName, temp.ObjectName);
                row[upload.ServerPathField] = config.AccessUrl;
                row[upload.ContentField] = config.ToString();

                temp.RemoveTo(config);
            }
            else
            {
                string fileName = Path.GetFileNameWithoutExtension(path);
                FileConfig config = new FileConfig(BucketName, fileName);
                row[upload.ServerPathField] = config.AccessUrl;
                row[upload.ContentField] = config.ToString();
                string originFileName = row[upload.FileNameField].ToString();

                config.UploadFile(path, originFileName);
            }
            return null;
        }

        protected override object UpdateAttachment(IDbDataSource host, IFieldUpload upload,
            IFieldValueAccessor accessor, string oldPath, string newPath)
        {
            if (UseAliyunUploadMode)
            {
                FileConfig temp = FileConfig.ReadFromJson(newPath);
                string json = accessor.GetValue(upload.ContentField);
                FileConfig config = FileConfig.ReadFromJson(json);
                accessor.SetValue(upload.ServerPathField, config.AccessUrl);

                temp.RemoveTo(config);
            }
            else
            {
                string json = accessor.GetValue(upload.ContentField);
                FileConfig config = FileConfig.ReadFromJson(json);
                accessor.SetValue(upload.ServerPathField, config.AccessUrl);
                string originFileName = accessor.GetValue(upload.FileNameField);

                config.UploadFile(newPath, originFileName);
            }
            return null;
        }

        protected override object UpdateAttachment(IDbDataSource host, IFieldUpload upload,
            DataRow row, string oldPath, string newPath)
        {
            if (UseAliyunUploadMode)
            {
                FileConfig temp = FileConfig.ReadFromJson(newPath);
                string json = row[upload.ContentField].ToString();
                FileConfig config = FileConfig.ReadFromJson(json);
                row[upload.ServerPathField] = config.AccessUrl;

                temp.RemoveTo(config);
            }
            else
            {
                string json = row[upload.ContentField].ToString();
                FileConfig config = FileConfig.ReadFromJson(json);
                row[upload.ServerPathField] = config.AccessUrl;
                string originFileName = row[upload.FileNameField].ToString();

                config.UploadFile(newPath, originFileName);
            }
            return null;
        }
    }
}