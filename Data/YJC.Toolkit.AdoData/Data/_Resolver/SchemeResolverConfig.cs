using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [ResolverCreatorConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-08-25", Description = "根据Scheme来生成TableResolver")]
    [ObjectContext]
    class SchemeResolverConfig : IConfigCreator<TableResolver>
    {
        #region IConfigCreator<TableResolver> 成员

        public TableResolver CreateObject(params object[] args)
        {
            IDbDataSource source = ObjectUtil.ConfirmQueryObject<IDbDataSource>(this, args);

            ITableScheme scheme = Scheme.CreateObject();
            return new TableResolver(scheme, source);
        }

        #endregion

        [DynamicElement(TableSchemeConfigFactory.REG_NAME, Required = true)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<ITableScheme> Scheme { get; private set; }
    }
}
