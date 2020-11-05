using System.Collections.Generic;
using System.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public sealed class ProcessConfig
    {
        public ProcessConfig()
        {
        }

        [ObjectElement]
        public UIOperationConfig UIOperation { get; internal set; }

        [ObjectElement(LocalName = "NonUIOperation", IsMultiple = true,
            ObjectType = typeof(NonUIOperationConfig), CollectionType = typeof(List<NonUIOperationConfig>))]
        public IEnumerable<NonUIOperationConfig> NonUIOperations { get; private set; }

        internal void AddNonUIOperationConfig(NonUIOperationConfig config)
        {
            if (NonUIOperations == null)
                NonUIOperations = new List<NonUIOperationConfig>();

            NonUIOperations.Convert<List<NonUIOperationConfig>>().Add(config);
        }

        internal DataTable CreateTable(bool containsSave)
        {
            UIOperation.ContainsSave = containsSave;
            DataTable table = EnumUtil.Convert(UIOperation).CreateTable("_Operation");

            if (NonUIOperations != null)
                foreach (NonUIOperationConfig config in NonUIOperations)
                {
                    DataRow row = table.NewRow();
                    config.AddToDataRow(row);
                    table.Rows.Add(row);
                }
            //DataTable table = DataSetUtil.CreateDataTable("_Operation", "Name",
            //    "DisplayName", "ButtonCaption", "PlugIn", "Type", "NeedPrompt");
            //DataRow uiRow = table.NewRow();
            //UIOperation.SetRowData(uiRow);
            //table.Rows.Add(uiRow);
            //foreach (NonUIOperationConfig operation in NonUIOperations)
            //{
            //    DataRow row = table.NewRow();
            //    operation.SetRowData(row);
            //    table.Rows.Add(row);
            //}
            return table;
        }
    }
}