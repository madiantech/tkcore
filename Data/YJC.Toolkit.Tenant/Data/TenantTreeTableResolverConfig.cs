using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [ResolverCreatorConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2018-05-22",
        Description = "在Tk5的DataXml基础上，支持Tenant的TreeTableResolver")]
    [ObjectContext]
    internal class TenantTreeTableResolverConfig : IConfigCreator<TableResolver>
    {
        #region IConfigCreator<TableResolver> 成员

        public TableResolver CreateObject(params object[] args)
        {
            IDbDataSource source = ObjectUtil.ConfirmQueryObject<IDbDataSource>(this, args);
            Tk5DataXml dataXml = TenantUtil.GetTk5DataXml(Scheme);

            return new TenantTreeTableResolver(dataXml, source)
            {
                AutoTrackField = AutoTrackField,
                AutoUpdateKey = AutoUpdateKey
            };
        }

        #endregion IConfigCreator<TableResolver> 成员

        [DynamicElement(TableSchemeExConfigFactory.REG_NAME, Required = true)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<ITableSchemeEx> Scheme { get; private set; }

        [SimpleAttribute]
        public bool AutoUpdateKey { get; private set; }

        [SimpleAttribute]
        public bool AutoTrackField { get; private set; }
    }
}