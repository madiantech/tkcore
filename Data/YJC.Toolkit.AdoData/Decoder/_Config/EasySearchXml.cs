using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    internal class EasySearchXml : ToolkitConfig
    {
        [XmlPlugInItem]
        [DynamicElement(EasySearchConfigFactory.REG_NAME, IsMultiple = true, Required = true)]
        public List<IXmlPlugInItem> EasySearches { get; private set; }
    }
}
