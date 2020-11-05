using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class EmptyUploadProcessor : IUploadProcessor, IConfigCreator<IUploadProcessor>
    {
        #region IUploadProcessor 成员

        public virtual object Process(IDbDataSource host, IFieldUpload upload, DataRow row, UpdateKind kind)
        {
            return null;
        }

        public string Display(IFieldUpload upload, DataRow row)
        {
            TkDebug.AssertArgumentNull(upload, "upload", this);
            TkDebug.AssertArgumentNull(row, "row", this);

            return string.Format(ObjectUtil.SysCulture, "<a href=\"{2}\" target=\"_blank\">{0}{1}</a>",
                row.GetString(upload.FileNameField), BaseUploadProcessor.FormatSize(row.GetValue<int>(upload.SizeField)),
                row.GetString(upload.ContentField));
        }

        public UploadInfo CreateValue(IFieldUpload upload, DataRow row)
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

        public IEnumerable<string> GetListSelectFields(TkDbContext context,
            IFieldUpload upload, IFieldInfoIndexer indexer)
        {
            return null;
        }

        #endregion IUploadProcessor 成员

        #region IConfigCreator<IUploadProcessor> 成员

        public IUploadProcessor CreateObject(params object[] args)
        {
            return this;
        }

        #endregion IConfigCreator<IUploadProcessor> 成员
    }
}