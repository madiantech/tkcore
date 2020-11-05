using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [ResolverCreatorConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-08-25", Description = "创建Tk5的TreeTableResolver")]
    class TreeResolverConfig : IConfigCreator<TableResolver>
    {
        #region IConfigCreator<TableResolver> 成员

        public TableResolver CreateObject(params object[] args)
        {
            IDbDataSource source = ObjectUtil.ConfirmQueryObject<IDbDataSource>(this, args);

            if (string.IsNullOrEmpty(TableName))
                return new Tk5TreeTableResolver(DataXml, source);
            else
                return new Tk5TreeTableResolver(DataXml, TableName, source);
        }

        #endregion

        [SimpleAttribute]
        public string DataXml { get; private set; }

        [SimpleAttribute]
        public string TableName { get; private set; }
    }
}
