using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    class OverrideListDetailConfig
    {
        [SimpleAttribute]
        public int? ListWidth { get; private set; }

        [SimpleAttribute]
        public bool? TextHead { get; set; }

        [SimpleAttribute]
        public bool? Span { get; private set; }

        [SimpleAttribute]
        public string SortField { get; private set; }

        [SimpleAttribute]
        public FieldSearchMethod? Search { get; private set; }

        [SimpleAttribute]
        public string Class { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, UseConstructor = true)]
        public Tk5LinkConfig Link { get; set; }

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(CoreDisplayConfigFactory.REG_NAME)]
        public IConfigCreator<IDisplay> ListDisplay { get; private set; }

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(CoreDisplayConfigFactory.REG_NAME)]
        public IConfigCreator<IDisplay> DetailDisplay { get; private set; }

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(ListSearchConfigFactory.REG_NAME)]
        public IConfigCreator<BaseListSearch> ListSearch { get; private set; }
    }
}
