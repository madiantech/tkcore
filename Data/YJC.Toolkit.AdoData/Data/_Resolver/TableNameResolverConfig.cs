using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [ResolverCreatorConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2013-10-22", Description = "根据配置的表名，主键和字段来生成TableResolver")]
    [ObjectContext]
    internal class TableNameResolverConfig : IConfigCreator<TableResolver>
    {
        #region IConfigCreator<TableResolver> 成员

        public TableResolver CreateObject(params object[] args)
        {
            IDbDataSource source = ObjectUtil.ConfirmQueryObject<IDbDataSource>(this, args);

            TkDebug.AssertNotNullOrEmpty(TableName, "没有配置TableName属性", this);
            TableResolver resolver;
            if (string.IsNullOrEmpty(KeyFields))
                resolver = new TableResolver(TableName, source);
            else if (string.IsNullOrEmpty(Fields))
                resolver = new TableResolver(TableName, KeyFields, source);
            else
                resolver = new TableResolver(TableName, KeyFields, Fields, source);
            resolver.AutoTrackField = AutoTrackField;
            resolver.AutoUpdateKey = AutoUpdateKey;

            return resolver;
        }

        #endregion

        [SimpleAttribute(Required = true)]
        public string TableName { get; private set; }

        [SimpleAttribute]
        public string KeyFields { get; private set; }

        [SimpleAttribute]
        public string Fields { get; private set; }

        [SimpleAttribute]
        public bool AutoUpdateKey { get; private set; }

        [SimpleAttribute]
        public bool AutoTrackField { get; private set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(TableName) ? base.ToString() :
                string.Format(ObjectUtil.SysCulture, "根据表{0}生产TableResolver的配置", TableName);
        }
    }
}
