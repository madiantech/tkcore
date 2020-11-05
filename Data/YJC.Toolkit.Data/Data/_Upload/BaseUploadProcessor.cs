using System;
using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public abstract class BaseUploadProcessor : IUploadProcessor, IUploadProcessor2
    {
        private const int KSIZE = 1024;
        private const int MSIZE = 1024 * 1024;

        protected BaseUploadProcessor()
        {
        }

        #region IUploadProcessor 成员

        public object Process(IDbDataSource host, IFieldUpload upload, DataRow row, UpdateKind kind)
        {
            switch (kind)
            {
                case UpdateKind.Insert:
                    string path = row[upload.ServerPathField].ToString();
                    if (!string.IsNullOrEmpty(path))
                        return InsertAttachment(host, upload, row, path);
                    break;

                case UpdateKind.Update:
                    string originalPath = row[upload.ServerPathField, DataRowVersion.Original].ToString();
                    string newPath = row[upload.ServerPathField].ToString();
                    if (originalPath != newPath)
                    {
                        if (string.IsNullOrEmpty(originalPath))
                        {
                            // 原先没有，当前实际是新建
                            return InsertAttachment(host, upload, row, newPath);
                        }
                        else if (string.IsNullOrEmpty(newPath))
                        {
                            // 现在没有，当前实际是删除
                            var result = DeleteAttachment(host, upload, row, originalPath);
                            row[upload.FileNameField] = DBNull.Value;
                            return result;
                        }
                        else
                        {
                            // 原先和现在都有，当前实际是覆盖
                            return UpdateAttachment(host, upload, row, originalPath, newPath);
                        }
                    }

                    break;

                case UpdateKind.Delete:
                    if (row[upload.ContentField] != DBNull.Value)
                        return DeleteAttachment(host, upload, row,
                            row[upload.ServerPathField].ToString());
                    break;
            }
            return null;
        }

        public abstract string Display(IFieldUpload upload, DataRow row);

        public abstract UploadInfo CreateValue(IFieldUpload upload, DataRow row);

        public abstract IEnumerable<string> GetListSelectFields(TkDbContext context,
            IFieldUpload upload, IFieldInfoIndexer indexer);

        #endregion IUploadProcessor 成员

        private static string GetUploadItemField(TkDbContext context, IFieldInfoIndexer indexer, string nickName)
        {
            var field = indexer[nickName];
            if (field == null)
                return null;
            return string.Format(ObjectUtil.SysCulture, "{0} {1}",
                context.EscapeName(field.FieldName), context.EscapeName(field.NickName));
        }

        protected abstract object InsertAttachment(IDbDataSource host, IFieldUpload upload,
            DataRow row, string path);

        protected abstract object UpdateAttachment(IDbDataSource host, IFieldUpload upload,
            DataRow row, string oldPath, string newPath);

        protected abstract object DeleteAttachment(IDbDataSource host, IFieldUpload upload,
            DataRow row, string path);

        internal static string FormatSize(int size)
        {
            if (size <= 0)
                return string.Empty;
            if (size < KSIZE)
                return string.Format(ObjectUtil.SysCulture, "({0:0.##}k)", size / (double)KSIZE);
            if (size < MSIZE)
                return string.Format(ObjectUtil.SysCulture, "({0:0.#}k)", size / (double)KSIZE);
            return string.Format(ObjectUtil.SysCulture, "({0:0.#}M)", size / (double)MSIZE);
        }

        protected IEnumerable<string> GetListSelectFields(TkDbContext context,
            IFieldUpload upload, IFieldInfoIndexer indexer, bool showSize, bool showContent,
            bool showMimeType, bool showServerPath)
        {
            TkDebug.AssertArgumentNull(upload, "upload", this);
            TkDebug.AssertArgumentNull(indexer, "indexer", this);

            string result;
            if (showSize)
            {
                result = GetUploadItemField(context, indexer, upload.SizeField);
                if (result != null)
                    yield return result;
            }
            if (showContent)
            {
                result = GetUploadItemField(context, indexer, upload.ContentField);
                if (result != null)
                    yield return result;
            }
            if (showMimeType)
            {
                result = GetUploadItemField(context, indexer, upload.MimeTypeField);
                if (result != null)
                    yield return result;
            }
            if (showServerPath)
            {
                result = GetUploadItemField(context, indexer, upload.ServerPathField);
                if (result != null)
                    yield return result;
            }
        }

        #region IUploadProcessor2 成员

        public object Process(IDbDataSource host, IFieldUpload upload, IFieldValueAccessor provider, UpdateKind kind)
        {
            switch (kind)
            {
                case UpdateKind.Insert:
                    string path = provider[upload.ServerPathField].ToString();
                    if (!string.IsNullOrEmpty(path))
                        return InsertAttachment(host, upload, provider, path);
                    break;

                case UpdateKind.Update:
                    string originalPath = provider.GetOriginValue(upload.ServerPathField).ToString();
                    string newPath = provider[upload.ServerPathField].ToString();
                    if (originalPath != newPath)
                    {
                        if (string.IsNullOrEmpty(originalPath))
                        {
                            // 原先没有，当前实际是新建
                            return InsertAttachment(host, upload, provider, newPath);
                        }
                        else if (string.IsNullOrEmpty(newPath))
                        {
                            // 现在没有，当前实际是删除
                            var result = DeleteAttachment(host, upload, provider, originalPath);
                            provider.SetValue(upload.FileNameField, DBNull.Value);
                            return result;
                        }
                        else
                        {
                            // 原先和现在都有，当前实际是覆盖
                            return UpdateAttachment(host, upload, provider, originalPath, newPath);
                        }
                    }

                    break;

                case UpdateKind.Delete:
                    if (provider[upload.ContentField] != DBNull.Value)
                        return DeleteAttachment(host, upload, provider,
                            provider[upload.ServerPathField].ToString());
                    break;
            }
            return null;
        }

        public abstract string Display(IFieldUpload upload, IFieldValueProvider provider);

        public abstract UploadInfo CreateValue(IFieldUpload upload, IFieldValueProvider provider);

        #endregion IUploadProcessor2 成员

        protected abstract object InsertAttachment(IDbDataSource host, IFieldUpload upload,
            IFieldValueAccessor accessor, string path);

        protected abstract object UpdateAttachment(IDbDataSource host, IFieldUpload upload,
            IFieldValueAccessor accessor, string oldPath, string newPath);

        protected abstract object DeleteAttachment(IDbDataSource host, IFieldUpload upload,
            IFieldValueAccessor accessor, string path);

        public virtual void AfterSave()
        {
        }
    }
}