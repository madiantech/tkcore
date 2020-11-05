using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public class OperationConfig
    {
        internal static readonly QName ROOT = QName.Get("Operation");

        [SimpleAttribute]
        public string Name { get; internal set; }

        [SimpleAttribute]
        public string DisplayName { get; internal set; }

        [SimpleAttribute]
        public string ButtonCaption { get; internal set; }

        [SimpleElement(UseCData = true)]
        public string PlugIn { get; internal set; }

        [SimpleAttribute]
        public virtual OperationType OperationType
        {
            get
            {
                return OperationType.NonUI;
            }
            protected set
            {
            }
        }

        [SimpleAttribute]
        public bool NeedPrompt { get; set; }

        public string CreateXml()
        {
            return this.WriteXml(WorkflowConst.WriteSettings, ROOT);
        }

        //internal virtual void SetRowData(DataRow row)
        //{
        //    row.BeginEdit();
        //    row["Name"] = Name;
        //    row["DisplayName"] = DisplayName;
        //    row["ButtonCaption"] = ButtonCaption;
        //    row["PlugIn"] = PlugIn;
        //    row["Type"] = (int)OperationType;
        //    row.EndEdit();
        //}

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(DisplayName) && !string.IsNullOrEmpty(Name))
            {
                string name = string.IsNullOrEmpty(DisplayName) ? Name : DisplayName;
                return string.Format(ObjectUtil.SysCulture, "操作{{{0}}}", name);
            }
            else
                return "无设置";
        }
    }
}