using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Log;

namespace YJC.Toolkit.Data
{
    public class DbCompositeSource : CompositeSource, ISupportRecordLog
    {
        #region ISupportRecordLog 成员

        public void SetRecordDataPicker(string tableName, IRecordDataPicker picker)
        {
            ISupportRecordLog log = CurrentSource as ISupportRecordLog;
            if (log != null)
                log.SetRecordDataPicker(tableName, picker);
        }

        public IEnumerable<RecordLogData> GetRecordDatas(string tableName)
        {
            ISupportRecordLog log = CurrentSource as ISupportRecordLog;
            if (log != null)
                return log.GetRecordDatas(tableName);

            return Enumerable.Empty<RecordLogData>();
        }

        #endregion ISupportRecordLog 成员
    }
}