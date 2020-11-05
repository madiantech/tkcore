using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [ObjectContext]
    [ResolverCreatorConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2017-10-31",
        Description = "根据Sql语句来生成TableResolver，这种Resolver可以展示数据，但是不具备回写数据库的能力")]
    internal class SqlTableResolverConfig : IConfigCreator<TableResolver>
    {
        [TagElement(NamespaceType.Toolkit, Required = true)]
        [DynamicElement(TableSchemeExConfigFactory.REG_NAME)]
        public IConfigCreator<ITableSchemeEx> Scheme { get; private set; }

        [SimpleElement(NamespaceType.Toolkit, Required = true)]
        public string Sql { get; private set; }

        public TableResolver CreateObject(params object[] args)
        {
            IDbDataSource source = ObjectUtil.ConfirmQueryObject<IDbDataSource>(this, args);

            ITableSchemeEx scheme = Scheme.CreateObject();
            SqlTableResolver resolver = new SqlTableResolver(Sql, scheme, source);
            return resolver;
        }
    }
}