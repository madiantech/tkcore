using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class StoredProcXml //: ToolkitConfigXml
    {
        [ObjectElement(NamespaceType.Toolkit)]
        public StoredProcConfigItem StoredProc { get; private set; }
    }
}