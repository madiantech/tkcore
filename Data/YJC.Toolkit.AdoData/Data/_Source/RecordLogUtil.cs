using System.Collections.Generic;
using YJC.Toolkit.Log;

namespace YJC.Toolkit.Data
{
    internal static class RecordLogUtil
    {
        public static void LogRecord(TableResolver resolver, IRecordDataPicker dataPicker,
            UpdatingEventArgs e, List<RecordLogData> logList)
        {
            try
            {
                var result = dataPicker.PickData(resolver, e);
                if (result != null)
                {
                    RecordLogData log = new RecordLogData(resolver.TableName,
                        e.InvokeMethod, e.Status, result);
                    logList.Add(log);
                }
            }
            catch
            {
            }
        }
    }
}