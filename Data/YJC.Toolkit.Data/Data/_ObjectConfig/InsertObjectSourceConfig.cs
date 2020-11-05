using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-11-20",
        Author = "YJC", Description = "提供对象新建时的Get和Post数据源")]
    class InsertObjectSourceConfig : BaseObjectSourceConfig<IInsertObjectSource>
    {
        protected override BaseObjectSource<IInsertObjectSource> CreateSource(IInsertObjectSource source)
        {
            return new InsertObjectSource(source);
        }
    }
}
