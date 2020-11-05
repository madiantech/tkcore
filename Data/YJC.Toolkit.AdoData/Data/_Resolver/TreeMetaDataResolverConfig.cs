using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [ResolverCreatorConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2018-05-22",
        Description = "基于ITableSchemeEx来创建相应的TreeTableResolver")]
    internal class TreeMetaDataResolverConfig : IConfigCreator<TableResolver>
    {
        #region IConfigCreator<TableResolver> 成员

        public TableResolver CreateObject(params object[] args)
        {
            IDbDataSource source = ObjectUtil.ConfirmQueryObject<IDbDataSource>(this, args);

            ITableSchemeEx scheme = Scheme.CreateObject();
            Tk5DataXml dataXml = scheme as Tk5DataXml;
            TkDebug.AssertNotNull(dataXml, string.Format(ObjectUtil.SysCulture,
                "模型需要Tk5DataXml，当前的Scheme是{0}，不适配", scheme.GetType()), Scheme);
            return new Tk5TreeTableResolver(dataXml, source)
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