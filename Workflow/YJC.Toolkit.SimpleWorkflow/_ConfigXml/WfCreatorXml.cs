using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class WfCreatorXml : ToolkitConfig
    {
        [ObjectElement(NamespaceType.Toolkit, Required = true)]
        [XmlPlugInItem]
        public CreatorConfigItem Creator { get; private set; }
    }
}