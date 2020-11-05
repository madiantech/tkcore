using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-01-03", Description = "提供对单表的Insert在Get的时候的数据源")]
    class SingleDbInsertSourceConfig : BaseResolverConfig, IEditDbConfig, ISingleResolverConfig
    {
        public override ISource CreateObject(params object[] args)
        {
            return new SingleDbInsertSource(this);
        }

        [SimpleAttribute]
        public bool UseMetaData { get; private set; }
    }
}
