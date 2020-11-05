using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    internal class CodeDataXml : ToolkitConfig
    {
        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "CodeData", Required = true)]
        [XmlPlugInItem]
        public List<CodeDataConfigItem> CodeDataList { get; private set; }
    }
}