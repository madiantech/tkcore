using System.Collections.Generic;
using System.Data;
using System.IO;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class DbUploadProcessor : BaseUploadProcessor
    {
        private const string DOWNLOAD_URL =
            "~/c/source/C/DownloadAttachment?NoFileName={2}&Cache={1}&Id={0}";

        private string fDelFile;

        public DbUploadProcessor()
            : this(true, true)
        {
        }

        public DbUploadProcessor(bool outputFileName, bool cacheFile)
        {
            OutputFileName = outputFileName;
            CacheFile = cacheFile;
        }

        internal DbUploadProcessor(DbUploadProcessorConfig config)
        {
            OutputFileName = config.OutputFileName;
            CacheFile = config.CacheFile;
        }

        public bool OutputFileName { get; set; }

        public bool CacheFile { get; set; }

        private static void DeleteTempFile(string path)
        {
            try
            {
                // 删除临时文件
                File.Delete(path);
            }
            catch
            {
            }
        }

        private string GetWebPath(string id)
        {
            string webPath = string.Format(ObjectUtil.SysCulture, DOWNLOAD_URL,
                id, CacheFile, !OutputFileName);
            return AppUtil.ResolveUrl(webPath);
        }

        protected override object InsertAttachment(IDbDataSource host,
            IFieldUpload upload, DataRow row, string path)
        {
            AttachmentResolver resolver = new AttachmentResolver(host);
            resolver.SetCommands(AdapterCommand.Insert);
            byte[] fileData = File.ReadAllBytes(path);
            row[upload.ContentField] = resolver.Insert(
                row[upload.FileNameField].ToString(), path,
                row[upload.MimeTypeField].ToString(), fileData, false);

            fDelFile = path;
            return resolver;
        }

        protected override object UpdateAttachment(IDbDataSource host,
            IFieldUpload upload, DataRow row, string oldPath, string newPath)
        {
            AttachmentResolver resolver = new AttachmentResolver(host);
            resolver.SetCommands(AdapterCommand.Update);
            resolver.Update(row[upload.ContentField].Value<int>(), row[upload.FileNameField].ToString(), newPath,
                row[upload.MimeTypeField].ToString(),
                File.ReadAllBytes(newPath), false);

            fDelFile = newPath;
            return resolver;
        }

        protected override object DeleteAttachment(IDbDataSource host,
            IFieldUpload upload, DataRow row, string path)
        {
            AttachmentResolver resolver = new AttachmentResolver(host);
            resolver.SetCommands(AdapterCommand.Delete);
            resolver.Delete(row[upload.ContentField].Value<int>(), false);
            return resolver;
        }

        public override string Display(IFieldUpload upload, DataRow row)
        {
            TkDebug.AssertArgumentNull(upload, "upload", this);
            TkDebug.AssertArgumentNull(row, "row", this);

            string url = GetWebPath(row.GetString(upload.ContentField));
            return string.Format(ObjectUtil.SysCulture, "<a href=\"{2}\" target=\"_blank\">{0}{1}</a>",
                StringUtil.EscapeHtml(row.GetString(upload.FileNameField)),
                FormatSize(row.GetValue<int>(upload.SizeField)),
                StringUtil.EscapeHtmlAttribute(url));
        }

        public override UploadInfo CreateValue(IFieldUpload upload, DataRow row)
        {
            TkDebug.AssertArgumentNull(upload, "upload", this);
            TkDebug.AssertArgumentNull(row, "row", this);

            string fileName = row.GetString(upload.FileNameField);
            if (string.IsNullOrEmpty(fileName))
                return null;

            string webPath = GetWebPath(row.GetString(upload.ContentField));
            UploadInfo info = new UploadInfo
            {
                FileName = fileName,
                ContentType = row.GetString(upload.MimeTypeField),
                ServerPath = row.GetString(upload.ServerPathField),
                FileSize = row.GetValue<int>(upload.SizeField),
                WebPath = webPath
            };
            return info;
        }

        public override IEnumerable<string> GetListSelectFields(TkDbContext context,
            IFieldUpload upload, IFieldInfoIndexer indexer)
        {
            return GetListSelectFields(context, upload, indexer, true, true, true, false);
        }

        public override string Display(IFieldUpload upload, IFieldValueProvider provider)
        {
            string url = GetWebPath(provider.GetValue(upload.ContentField));

            return url;
        }

        public override UploadInfo CreateValue(IFieldUpload upload, IFieldValueProvider provider)
        {
            TkDebug.AssertArgumentNull(upload, "upload", this);
            TkDebug.AssertArgumentNull(provider, "provider", this);

            string fileName = provider.GetValue(upload.FileNameField);
            if (string.IsNullOrEmpty(fileName))
                return null;

            string webPath = GetWebPath(provider.GetValue(upload.ContentField));
            UploadInfo info = new UploadInfo
            {
                FileName = fileName,
                ContentType = provider.GetValue(upload.MimeTypeField),
                ServerPath = provider.GetValue(upload.ServerPathField),
                FileSize = provider.GetValue<int>(upload.SizeField),
                WebPath = webPath
            };
            return info;
        }

        protected override object InsertAttachment(IDbDataSource host, IFieldUpload upload,
            IFieldValueAccessor accessor, string path)
        {
            AttachmentResolver resolver = new AttachmentResolver(host);
            resolver.SetCommands(AdapterCommand.Insert);
            byte[] fileData = File.ReadAllBytes(path);
            accessor.SetValue(upload.ContentField, resolver.Insert(
                accessor[upload.FileNameField].ToString(), path,
                accessor[upload.MimeTypeField].ToString(), fileData, false));

            fDelFile = path;
            return resolver;
        }

        protected override object UpdateAttachment(IDbDataSource host, IFieldUpload upload,
            IFieldValueAccessor accessor, string oldPath, string newPath)
        {
            AttachmentResolver resolver = new AttachmentResolver(host);
            resolver.SetCommands(AdapterCommand.Update);
            resolver.Update(accessor[upload.ContentField].Value<int>(),
                accessor[upload.FileNameField].ToString(), newPath,
                accessor[upload.MimeTypeField].ToString(),
                File.ReadAllBytes(newPath), false);

            fDelFile = newPath;
            return resolver;
        }

        protected override object DeleteAttachment(IDbDataSource host, IFieldUpload upload,
            IFieldValueAccessor accessor, string path)
        {
            AttachmentResolver resolver = new AttachmentResolver(host);
            resolver.SetCommands(AdapterCommand.Delete);
            resolver.Delete(accessor[upload.ContentField].Value<int>(), false);
            return resolver;
        }

        public override void AfterSave()
        {
            if (!string.IsNullOrEmpty(fDelFile))
                DeleteTempFile(fDelFile);
        }
    }
}