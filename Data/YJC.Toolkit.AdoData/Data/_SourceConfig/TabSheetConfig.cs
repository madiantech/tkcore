using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    class TabSheetConfig
    {
        [SimpleAttribute]
        public string Id { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public MultiLanguageText Caption { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public MarcoConfigItem Condition { get; private set; }
    }
}
