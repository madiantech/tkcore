using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    internal class CodeTableXml : ToolkitConfig
    {
        [XmlPlugInItem]
        [DynamicElement(CodeTableConfigFactory.REG_NAME, IsMultiple = true, Required = true)]
        public List<IXmlPlugInItem> CodeTables { get; private set; }
    }
}