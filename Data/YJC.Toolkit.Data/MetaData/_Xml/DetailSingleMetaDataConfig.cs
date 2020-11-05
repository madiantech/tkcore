using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    public class DetailSingleMetaDataConfig : BaseMDSingleMetaDataConfig
    {
        [SimpleAttribute]
        public bool IsFix { get; protected set; }

        [SimpleAttribute(DefaultValue = TableShowStyle.None)]
        public TableShowStyle ListStyle { get; protected set; }

        [DynamicElement(TableOutputConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<ITableOutput> TableOutput { get; protected set; }
    }
}