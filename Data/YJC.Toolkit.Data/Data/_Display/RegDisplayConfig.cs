using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [CoreDisplayConfig(CreateDate = "2015-11-23", NamespaceType = NamespaceType.Toolkit,
        Author = "YJC", Description = "根据配置的注册名实例化Display")]
    [ObjectContext]
    internal class RegDisplayConfig : RegFactoryConfig<IDisplay>
    {
        public RegDisplayConfig()
            : base(DisplayPlugInFactory.REG_NAME)
        {
        }
    }
}
