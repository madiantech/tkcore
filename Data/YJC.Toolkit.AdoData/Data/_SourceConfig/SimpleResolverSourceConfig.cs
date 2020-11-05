using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-12-18", Description = "通过TableResolver获取整个表数据，支持解码的数据源")]
    internal class SimpleResolverSourceConfig : IConfigCreator<ISource>
    {
        #region IConfigCreator<ISource> 成员

        public ISource CreateObject(params object[] args)
        {
            return new SimpleResolverSource(this);
        }

        #endregion

        [SimpleAttribute]
        public string OrderBy { get; protected set; }

        [SimpleAttribute]
        public bool UserCallerInfo { get; protected set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public MarcoConfigItem FilterSql { get; protected set; }

        [DynamicElement(ResolverCreatorConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<TableResolver> Resolver { get; protected set; }
    }
}
