using YJC.Toolkit.Collections;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    class ResourceTableConfig
    {
        [SimpleAttribute]
        public string TableName { get; set; }

        [SimpleAttribute]
        public string NameField { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Field")]
        public RegNameList<ResourceFieldInfo> FieldList { get; private set; }
    }
}
