using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [ObjectContext]
    [ResolverCreatorConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2019-10-13",
        Description = "根据Sql语句来生成TableResolver，对应的TableScheme也由Sql生成。这种Resolver可适合无界面的数据操作，不具备回写数据库的能力")]
    internal class SimpleSqlTableResolverConfig : IConfigCreator<TableResolver>
    {
        [SimpleAttribute(Required = true)]
        public string TableName { get; private set; }

        [SimpleAttribute(Required = true)]
        public string KeyFields { get; private set; }

        [SimpleAttribute]
        public string Context { get; private set; }

        [SimpleElement(NamespaceType = NamespaceType.Toolkit, Required = true)]
        public string Sql { get; private set; }

        public TableResolver CreateObject(params object[] args)
        {
            IDbDataSource source = ObjectUtil.ConfirmQueryObject<IDbDataSource>(this, args);
            TkDbContext context = string.IsNullOrEmpty(Context) ? DbContextUtil.CreateDefault()
                : DbContextUtil.CreateDbContext(Context);
            using (context)
            {
                var scheme = DbUtil.CreateSqlTableScheme(Sql, TableName, KeyFields, context);
                var schemeEx = MetaDataUtil.ConvertToTableSchemeEx(scheme);
                return new SqlTableResolver(Sql, schemeEx, source);
            }
        }
    }
}