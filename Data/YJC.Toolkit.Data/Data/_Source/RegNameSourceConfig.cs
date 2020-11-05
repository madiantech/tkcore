using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2013-10-10",
        Description = "根据配置的注册名实例化相应的数据源")]
    internal sealed class RegNameSourceConfig : RegFactoryConfig<ISource>
    {
        public RegNameSourceConfig()
            : base(SourcePlugInFactory.REG_NAME)
        {
        }
    }
}
