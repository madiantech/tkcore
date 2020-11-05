using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class CreatorConfigItem : BaseXmlPlugInItem
    {
        internal const string BASE_CLASS = "Default";

        [SimpleAttribute(DefaultValue = FillContentMode.None)]
        public FillContentMode FillMode { get; private set; }

        [SimpleElement(NamespaceType.Toolkit, Required = true)]
        public string WorkflowName { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Content")]
        public List<ContentConfigItem> ContentList { get; private set; }

        public override string BaseClass
        {
            get
            {
                return BASE_CLASS;
            }
        }
    }
}