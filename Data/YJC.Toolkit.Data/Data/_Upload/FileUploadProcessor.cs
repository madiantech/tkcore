using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class FileUploadProcessor : BaseUploadProcessor
    {
        private readonly string fVirtualPath;
        private readonly string fUploadPath;
        private string fDelFile;

        public FileUploadProcessor(string virtualPath, string uploadPath)
        {
            TkDebug.AssertArgumentNullOrEmpty(virtualPath, "virtualPath", null);
            TkDebug.AssertArgumentNullOrEmpty(uploadPath, "uploadPath", null);

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);
            if (virtualPath.EndsWith("/", StringComparison.Ordinal))
                fVirtualPath = virtualPath;
            else
                fVirtualPath = virtualPath + "/";
            fUploadPath = uploadPath;
        }

        internal FileUploadProcessor(FileUploadProcessorConfig config)
            : this(config.CreateVirtualPath(), config.CreateUploadPath())
        {
        }

        protected override object InsertAttachment(IDbDataSource host, IFieldUpload upload,
            DataRow row, string path)
        {
            string fileName = Path.GetFileName(path);
            string newPath = Path.Combine(fUploadPath, fileName);
            row[upload.ServerPathField] = newPath;
            row[upload.ContentField] = fVirtualPath + fileName;

            CopyFile(path, newPath);
            return null;
        }

        protected override object UpdateAttachment(IDbDataSource host, IFieldUpload upload,
            DataRow row, string oldPath, string newPath)
        {
            string fileName = Path.GetFileName(newPath);
            row[upload.ContentField] = fVirtualPath + fileName;
            //Add By Gaolq 2018/8/15
            string toPath = Path.Combine(fUploadPath, fileName);

            row[upload.ServerPathField] = toPath;
            CopyFile(newPath, toPath);
            return null;
        }

        protected override object DeleteAttachment(IDbDataSource host, IFieldUpload upload,
            DataRow row, string path)
        {
            try
            {
                File.Delete(path);
            }
            catch
            {
            }
            return null;
        }

        public override string Display(IFieldUpload upload, DataRow row)
        {
            TkDebug.AssertArgumentNull(upload, "upload", this);
            TkDebug.AssertArgumentNull(row, "row", this);

            return string.Format(ObjectUtil.SysCulture, "<a href=\"{2}\" target=\"_blank\">{0}{1}</a>",
                StringUtil.EscapeHtml(row.GetString(upload.FileNameField)),
                FormatSize(row.GetValue<int>(upload.SizeField)),
                StringUtil.EscapeHtmlAttribute(row.GetString(upload.ContentField)));
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
                WebPath = row.GetString(upload.ContentField)
            };
            return info;
        }

        public override IEnumerable<string> GetListSelectFields(TkDbContext context,
            IFieldUpload upload, IFieldInfoIndexer indexer)
        {
            return GetListSelectFields(context, upload, indexer, true, true, true, true);
        }

        private void CopyFile(string srcPath, string dstPath)
        {
            try
            {
                File.Copy(srcPath, dstPath, true);

                fDelFile = srcPath;
            }
            catch
            {
            }
        }

        public override string Display(IFieldUpload upload, IFieldValueProvider provider)
        {
            return provider.GetValue(upload.ContentField);
        }

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
                WebPath = provider.GetValue(upload.ContentField)
            };
            return info;
        }

        protected override object InsertAttachment(IDbDataSource host, IFieldUpload upload,
            IFieldValueAccessor accessor, string path)
        {
            string fileName = Path.GetFileName(path);
            string newPath = Path.Combine(fUploadPath, fileName);
            accessor.SetValue(upload.ServerPathField, newPath);
            accessor.SetValue(upload.ContentField, fVirtualPath + fileName);

            CopyFile(path, newPath);
            return null;
        }

        protected override object UpdateAttachment(IDbDataSource host, IFieldUpload upload,
            IFieldValueAccessor accessor, string oldPath, string newPath)
        {
            string fileName = Path.GetFileName(newPath);
            accessor.SetValue(upload.ContentField, fVirtualPath + fileName);
            //Add By Gaolq 2018/8/15
            string toPath = Path.Combine(fUploadPath, fileName);
            accessor.SetValue(upload.ServerPathField, toPath);

            CopyFile(newPath, toPath);
            return null;
        }

        protected override object DeleteAttachment(IDbDataSource host, IFieldUpload upload,
            IFieldValueAccessor accessor, string path)
        {
            try
            {
                accessor.SetValue(upload.ContentField, DBNull.Value);
                File.Delete(path);
            }
            catch
            {
            }
            return null;
        }

        public override void AfterSave()
        {
            try
            {
                File.Delete(fDelFile);
            }
            catch
            {
            }
        }
    }
}