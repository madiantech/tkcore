using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data
{
    public interface IUploadProcessor
    {
        object Process(IDbDataSource host, IFieldUpload upload,
            DataRow row, UpdateKind kind);

        string Display(IFieldUpload upload, DataRow row);

        UploadInfo CreateValue(IFieldUpload upload, DataRow row);

        IEnumerable<string> GetListSelectFields(TkDbContext context,
            IFieldUpload upload, IFieldInfoIndexer indexer);
    }
}