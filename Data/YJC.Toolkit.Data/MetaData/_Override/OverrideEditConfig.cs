using System.Collections.Generic;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    class OverrideEditConfig
    {
        [SimpleAttribute]
        public bool? ReadOnly { get; private set; }

        [SimpleAttribute]
        public string Class { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public MarcoConfigItem DefaultValue { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        internal UpdatingConfigItem Updating { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Attribute")]
        public List<HtmlAttribute> AttributeList { get; private set; }

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(CoreDisplayConfigFactory.REG_NAME)]
        public IConfigCreator<IDisplay> Display { get; private set; }
    }
}
