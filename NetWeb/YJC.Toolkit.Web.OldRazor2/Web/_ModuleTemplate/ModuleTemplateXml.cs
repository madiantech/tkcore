using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class ModuleTemplateXml : ToolkitConfig
    {
        [XmlPlugInItem]
        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "ModuleTemplate")]
        public List<ModuleTemplateConfigItem> ModuleTemplates { get; private set; }
    }
}