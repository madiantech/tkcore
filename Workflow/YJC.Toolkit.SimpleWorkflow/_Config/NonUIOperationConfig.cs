using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public sealed class NonUIOperationConfig : OperationConfig
    {
        //internal override void SetRowData(DataRow row)
        //{
        //    base.SetRowData(row);
        //    row["NeedPrompt"] = NeedPrompt;
        //}

        public static NonUIOperationConfig ReadFromXml(string xml)
        {
            NonUIOperationConfig content = new NonUIOperationConfig();
            content.ReadXml(xml, WorkflowConst.ReadSettings, ROOT);
            return content;
        }
    }
}