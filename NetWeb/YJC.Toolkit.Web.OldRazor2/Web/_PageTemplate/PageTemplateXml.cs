using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class PageTemplateXml : ToolkitConfig
    {
        [XmlPlugInItem]
        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "PageTemplate")]
        public List<PageTemplateConfigItem> PageTemplates { get; private set; }
    }
}