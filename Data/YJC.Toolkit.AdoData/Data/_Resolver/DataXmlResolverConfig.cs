using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [ResolverCreatorConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2013-10-22", Description = "根据DataXml来生成Tk5TableResolver")]
    [ObjectContext]
    internal class DataXmlResolverConfig : IConfigCreator<TableResolver>
    {
        #region IConfigCreator<TableResolver> 成员

        public TableResolver CreateObject(params object[] args)
        {
            IDbDataSource source = ObjectUtil.ConfirmQueryObject<IDbDataSource>(this, args);

            Tk5TableResolver resolver;
            if (string.IsNullOrEmpty(TableName))
                resolver = new Tk5TableResolver(DataXml, source);
            else
                resolver = new Tk5TableResolver(DataXml, TableName, source);
            resolver.AutoUpdateKey = AutoUpdateKey;
            resolver.AutoTrackField = AutoTrackField;

            return resolver;
        }

        #endregion

        [SimpleAttribute(Required = true)]
        public string DataXml { get; private set; }

        [SimpleAttribute]
        public string TableName { get; private set; }

        [SimpleAttribute]
        public bool AutoUpdateKey { get; private set; }

        [SimpleAttribute]
        public bool AutoTrackField { get; private set; }
    }
}
