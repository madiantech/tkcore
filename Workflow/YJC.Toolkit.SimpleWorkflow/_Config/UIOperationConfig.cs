using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public sealed class UIOperationConfig : OperationConfig
    {
        [SimpleAttribute]
        public bool ContainsSave { get; set; }

        [SimpleAttribute]
        public override OperationType OperationType
        {
            get
            {
                return OperationType.UI;
            }
            protected set
            {
            }
        }

        public static UIOperationConfig ReadFromXml(string xml)
        {
            UIOperationConfig content = new UIOperationConfig();
            content.ReadXml(xml, WorkflowConst.ReadSettings, ROOT);
            return content;
        }
    }
}