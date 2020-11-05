using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-01-06", Description = "提供对单表的Delete数据源")]
    class SingleDbDeleteSourceConfig : BaseResolverConfig, IEditDbConfig
    {
        public override ISource CreateObject(params object[] args)
        {
            return new SingleDbDeleteSource(this);
        }

        #region IEditDbConfig 成员

        public bool UseMetaData { get; private set; }

        #endregion
    }
}
