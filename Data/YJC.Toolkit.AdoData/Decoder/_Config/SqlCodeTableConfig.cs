using YJC.Toolkit.Cache;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    [CodeTableConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-01-19",
        Author = "YJC", Description = "由一条SQL构成的CodeTable")]
    [ObjectContext]
    internal class SqlCodeTableConfig : BaseXmlPlugInItem
    {
        public const string BASE_CLASS = "SqlCodeTable";

        #region IXmlPlugInItem 成员

        public override string BaseClass
        {
            get
            {
                return BASE_CLASS;
            }
        }

        #endregion

        [SimpleAttribute]
        public string Context { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, Required = true)]
        public MarcoConfigItem Sql { get; private set; }

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(CacheDependencyConfigFactory.REG_NAME)]
        public IConfigCreator<ICacheDependency> CacheDependency { get; private set; }
    }
}
