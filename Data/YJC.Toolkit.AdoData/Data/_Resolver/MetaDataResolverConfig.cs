using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [ResolverCreatorConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2017-11-01",
        Description = "根据返回的ITableSchemeEx创建MetaDataResolver，要求对返回的ITableSchemeEx进行类型判断，如果是Tk5DataXml创建Tk5TableResolver，否则创建MetaDataTableResolver")]
    [ObjectContext]
    internal class MetaDataResolverConfig : IConfigCreator<TableResolver>
    {
        [DynamicElement(TableSchemeExConfigFactory.REG_NAME, Required = true)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<ITableSchemeEx> Scheme { get; private set; }

        [SimpleAttribute]
        public bool AutoUpdateKey { get; private set; }

        [SimpleAttribute]
        public bool AutoTrackField { get; private set; }

        public TableResolver CreateObject(params object[] args)
        {
            IDbDataSource source = ObjectUtil.ConfirmQueryObject<IDbDataSource>(this, args);

            ITableSchemeEx scheme = Scheme.CreateObject();
            Tk5DataXml dataXml = scheme as Tk5DataXml;
            if (dataXml == null)
                return new MetaDataTableResolver(scheme, source)
                {
                    AutoTrackField = AutoTrackField,
                    AutoUpdateKey = AutoUpdateKey
                };
            else
                return new Tk5TableResolver(dataXml, source)
                {
                    AutoTrackField = AutoTrackField,
                    AutoUpdateKey = AutoUpdateKey
                };
        }
    }
}