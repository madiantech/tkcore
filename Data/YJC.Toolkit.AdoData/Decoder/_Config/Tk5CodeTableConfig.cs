using YJC.Toolkit.Cache;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    [CodeTableConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-01-12",
        Author = "YJC", Description = "支持Tk5的DataXml的代码表配置")]
    [ObjectContext]
    internal class Tk5CodeTableConfig : BaseXmlPlugInItem
    {
        public const string BASE_CLASS = "Tk5CodeTable";

        #region IXmlPlugInItem 成员

        public override string BaseClass
        {
            get
            {
                return BASE_CLASS;
            }
        }

        #endregion

        [SimpleAttribute(Required = true)]
        public string DataXml { get; private set; }

        [SimpleAttribute]
        public string Context { get; private set; }

        [SimpleAttribute]
        public string OrderBy { get; private set; }

        [SimpleAttribute]
        public string NameExpression { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public MarcoConfigItem FilterSql { get; private set; }

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(CacheDependencyConfigFactory.REG_NAME)]
        public IConfigCreator<ICacheDependency> CacheDependency { get; private set; }
    }
}