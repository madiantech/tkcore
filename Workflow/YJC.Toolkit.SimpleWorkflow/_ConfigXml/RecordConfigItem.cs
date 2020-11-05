using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class RecordConfigItem
    {
        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, Required = true, LocalName = "Key")]
        public List<KeyConfigItem> KeyList { get; private set; }
    }
}