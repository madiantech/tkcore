using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Excel
{
    internal class ImportConfigItem
    {
        [ObjectElement(NamespaceType.Toolkit)]
        public MultiLanguageText Title { get; private set; }

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(MetaDataConfigFactory.REG_NAME)]
        public IConfigCreator<IMetaData> MetaData { get; private set; }

        [DynamicElement(ResolverCreatorConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<TableResolver> Resolver { get; protected set; }
    }
}
