using System.Collections.Generic;
using YJC.Toolkit.Data;

namespace YJC.Toolkit.Sys
{
    internal class ConsoleAppXml : ToolkitConfig
    {
        [ObjectElement(NamespaceType.Toolkit)]
        public ApplicationConfigItem Application { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public DebugConfigItem Debug { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Database")]
        [TagElement(NamespaceType.Toolkit)]
        public List<DbContextConfig> Databases { get; private set; }

        [TagElement(NamespaceType.Toolkit)]
        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Host")]
        public List<HostConfigItem> Hosts { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit, UseConstructor = true)]
        public SecretKey SecretKey { get; private set; }
    }
}