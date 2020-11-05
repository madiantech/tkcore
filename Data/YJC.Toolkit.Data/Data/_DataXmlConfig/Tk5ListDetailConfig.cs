using System;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [Serializable]
    public sealed class Tk5ListDetailConfig
    {
        [SimpleAttribute]
        public int ListWidth { get; private set; }

        [SimpleAttribute]
        public bool TextHead { get; internal set; }

        [SimpleAttribute]
        public bool Span { get; set; }

        [SimpleAttribute]
        public string SortField { get; private set; }

        [SimpleAttribute]
        public FieldSearchMethod Search { get; set; }

        [SimpleAttribute]
        public string Class { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, UseConstructor = true)]
        public Tk5LinkConfig Link { get; set; }

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(CoreDisplayConfigFactory.REG_NAME)]
        public IConfigCreator<IDisplay> ListDisplay { get; set; }

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(CoreDisplayConfigFactory.REG_NAME)]
        public IConfigCreator<IDisplay> DetailDisplay { get; set; }

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(ListSearchConfigFactory.REG_NAME)]
        public IConfigCreator<BaseListSearch> ListSearch { get; private set; }

        internal void Override(OverrideListDetailConfig config)
        {
            if (config.ListWidth.HasValue)
                ListWidth = config.ListWidth.Value;
            if (config.TextHead.HasValue)
                TextHead = config.TextHead.Value;
            if (config.Span.HasValue)
                Span = config.Span.Value;
            if (!string.IsNullOrEmpty(config.SortField))
                SortField = config.SortField;
            if (config.Search.HasValue)
                Search = config.Search.Value;
            if (!string.IsNullOrEmpty(config.Class))
                Class = config.Class;
            if (config.Link != null)
            {
                Link = config.Link;
                Link.ProcessType();
            }
            if (config.ListDisplay != null)
                ListDisplay = config.ListDisplay;
            if (config.DetailDisplay != null)
                DetailDisplay = config.DetailDisplay;
            if (config.ListSearch != null)
                ListSearch = config.ListSearch;
        }

        public static Tk5ListDetailConfig Clone(Tk5ListDetailConfig config)
        {
            if (config == null)
                return null;

            Tk5ListDetailConfig result = new Tk5ListDetailConfig()
            {
                ListWidth = config.ListWidth,
                TextHead = config.TextHead,
                Span = config.Span,
                SortField = config.SortField,
                Search = config.Search,
                Class = config.Class,
                Link = config.Link,
                ListDisplay = config.ListDisplay,
                DetailDisplay = config.DetailDisplay,
                ListSearch = config.ListSearch
            };
            return result;
        }
    }
}