using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-11-20",
        Author = "YJC", Description = "提供对象列表时的数据源")]
    class ListObjectSourceConfig : BaseObjectSourceConfig<IListObjectSource>
    {
        [SimpleAttribute]
        public int PageSize { get; private set; }

        [DynamicElement(ObjectOperatorsConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<IObjectOperatorsConfig> Operators { get; private set; }

        protected override BaseObjectSource<IListObjectSource> CreateSource(IListObjectSource source)
        {
            ListObjectSource result = new ListObjectSource(source, this);
            return result;
        }
    }
}
