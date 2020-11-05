using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(Description = "根据具体条件来选择相应配置的数据源，支持Db的附加操作",
        NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2019-01-02")]
    internal class DbCompositeSourceConfig : CompositeSourceConfig
    {
        protected override CompositeSource CreateSource()
        {
            return new DbCompositeSource();
        }
    }
}