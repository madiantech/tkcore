using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class ProcessXml : ToolkitConfig
    {
        public ProcessXml()
        {
        }

        public ProcessXml(FillContentMode mode)
            : this()
        {
            FillMode = mode;
        }

        [SimpleElement(NamespaceType.Toolkit, DefaultValue = FillContentMode.All)]
        public FillContentMode? FillMode { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Table")]
        public List<ProcessTableDataConfig> TableList { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public WfPageMakerConfig PageMaker { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Content")]
        public List<ProcessContentConfigItem> ContentList { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "RecordLog")]
        public List<RecordLogConfigItem> RecordLogs { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Relation")]
        public List<RelationConfigItem> Relations { get; protected set; }

        protected override void OnReadObject()
        {
            if (FillMode == null)
                FillMode = FillContentMode.All;
        }
    }
}