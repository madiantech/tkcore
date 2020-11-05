using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2014-08-08",
        Description = "从表中选出数据后全部删除，再全部插入提交的数据源。适用于多对多关系的连接表操作")]
    class SingleDbDelInsSourceConfig : BaseResolverConfig
    {
        [ObjectElement(NamespaceType.Toolkit)]
        public MarcoConfigItem FilterSql { get; private set; }

        public override ISource CreateObject(params object[] args)
        {
            return new SingleDbDelInsSource(this) { FilterSql = FilterSql };
        }
    }
}
