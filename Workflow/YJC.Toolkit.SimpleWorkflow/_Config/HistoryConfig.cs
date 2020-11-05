using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public class HistoryConfig
    {
        [SimpleAttribute]
        public bool IsSaveHistory { get; internal set; }

        [ObjectElement(IsMultiple = true, LocalName = "Table", ObjectType = typeof(HistoryTableConfig))]
        public List<HistoryTableConfig> HistoryTables { get; internal set; }
    }
}