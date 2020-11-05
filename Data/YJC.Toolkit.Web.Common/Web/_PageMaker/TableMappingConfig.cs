using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class TableMappingConfig
    {
        [SimpleAttribute]
        public string TableName { get; private set; }

        [SimpleElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "RemoveField")]
        public List<string> RemoveFields { get; private set; }
    }
}
