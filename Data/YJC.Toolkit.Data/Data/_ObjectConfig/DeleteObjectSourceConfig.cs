using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-11-20",
        Author = "YJC", Description = "提供对象删除时的数据源")]
    class DeleteObjectSourceConfig : BaseObjectSourceConfig<IDeleteObjectSource>
    {
        protected override BaseObjectSource<IDeleteObjectSource> CreateSource(IDeleteObjectSource source)
        {
            return new DeleteObjectSource(source);
        }
    }
}
