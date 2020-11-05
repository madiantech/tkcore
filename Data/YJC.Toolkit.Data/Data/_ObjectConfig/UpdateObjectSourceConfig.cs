using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-11-20",
        Author = "YJC", Description = "提供对象修改时的Get和Post数据源")]
    class UpdateObjectSourceConfig : BaseObjectSourceConfig<IUpdateObjectSource>
    {
        protected override BaseObjectSource<IUpdateObjectSource> CreateSource(IUpdateObjectSource source)
        {
            return new UpdateObjectSource(source);
        }
    }
}
