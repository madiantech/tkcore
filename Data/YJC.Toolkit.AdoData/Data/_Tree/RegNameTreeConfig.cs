using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [TreeConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2014-11-21",
        Description = "根据配置的注册名实例化Tree")]
    internal sealed class RegNameTreeConfig : RegFactoryConfig<ITree>
    {
        public RegNameTreeConfig()
            : base(TreePlugInFactory.REG_NAME)
        {
        }
    }
}
