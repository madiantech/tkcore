using YJC.Toolkit.Cache;
using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    internal abstract class BaseEasySearchConfig : BaseXmlPlugInItem
    {
        protected BaseEasySearchConfig()
        {
        }

        [SimpleAttribute]
        public string Context { get; protected set; }

        [SimpleAttribute]
        public string OrderBy { get; protected set; }

        [SimpleAttribute]
        public string NameExpression { get; protected set; }

        [SimpleAttribute]
        public string DisplayNameExpression { get; protected set; }

        [SimpleAttribute]
        public int TopCount { get; protected set; }

        [SimpleAttribute]
        public string PyField { get; protected set; }

        [SimpleAttribute]
        public string IdField { get; protected set; }

        [SimpleAttribute]
        public string NameField { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public MarcoConfigItem FilterSql { get; protected set; }

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(CacheDependencyConfigFactory.REG_NAME)]
        public IConfigCreator<ICacheDependency> CacheDependency { get; protected set; }

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(DataRightConfigFactory.REG_NAME)]
        public IConfigCreator<IDataRight> DataRight { get; protected set; }

        [SimpleElement(NamespaceType.Toolkit)]
        public string SearchMethod { get; protected set; }

        public abstract ITableSchemeEx CreateScheme();
    }
}