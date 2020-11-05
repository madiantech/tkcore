using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2015-07-29", Description = "单独维护整个子表的数据源")]
    internal class DbDetailTableMaintainSourceConfig : BaseResolverConfig,
        ISingleResolverConfig, IEditDbConfig
    {
        [SimpleAttribute]
        public bool UseMetaData { get; private set; }

        [SimpleAttribute(DefaultValue = UpdateMode.Merge)]
        public UpdateMode UpdateMode { get; protected set; }

        [SimpleAttribute]
        public string ParentKey { get; private set; }

        [SimpleAttribute]
        public string QueryStringName { get; private set; }

        public override ISource CreateObject(params object[] args)
        {
            DbDetailTableMaintainSource source = new DbDetailTableMaintainSource(this);
            return source;
        }
    }
}
