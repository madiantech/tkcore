using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-11-20",
        Author = "YJC", Description = "提供对象查看时的数据源")]
    class DetailObjectSourceConfig : BaseObjectSourceConfig<IDetailObjectSource>
    {
        [DynamicElement(ObjectOperatorsConfigFactory.REG_NAME)]
        [TagElement(NamespaceType.Toolkit)]
        public IConfigCreator<IObjectOperatorsConfig> DetailOperators { get; private set; }

        protected override BaseObjectSource<IDetailObjectSource> CreateSource(IDetailObjectSource source)
        {
            DetailObjectSource result = new DetailObjectSource(source);
            if (DetailOperators != null)
                result.Operators = DetailOperators.CreateObject();
            return result;
        }
    }
}
