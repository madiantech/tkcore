using System.Collections.Generic;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data
{
    public interface IUploadProcessor2
    {
        object Process(IDbDataSource host, IFieldUpload upload,
           IFieldValueAccessor accessor, UpdateKind kind);

        string Display(IFieldUpload upload, IFieldValueProvider provider);

        UploadInfo CreateValue(IFieldUpload upload, IFieldValueProvider provider);

        IEnumerable<string> GetListSelectFields(TkDbContext context,
            IFieldUpload upload, IFieldInfoIndexer indexer);

        void AfterSave();
    }
}