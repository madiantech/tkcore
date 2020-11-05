using System.Collections.Generic;
using YJC.Toolkit.Data;

namespace YJC.Toolkit.Sys
{
    internal class WebAppXml : ToolkitConfig
    {
        [ObjectElement(NamespaceType.Toolkit)]
        public ApplicationConfigItem Application { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public WebDebugConfigItem Debug { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Database")]
        [TagElement(NamespaceType.Toolkit)]
        public List<DbContextConfig> Databases { get; protected set; }

        [TagElement(NamespaceType.Toolkit)]
        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Host")]
        public List<HostConfigItem> Hosts { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit, UseConstructor = true)]
        public SecretKey SecretKey { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public IOConfigItem IO { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public UploadConfigItem Upload { get; protected set; }
    }
}