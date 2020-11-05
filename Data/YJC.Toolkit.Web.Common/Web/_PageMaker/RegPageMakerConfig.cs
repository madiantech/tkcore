using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2013-10-17",
        Description = "根据配置的注册名实例化PageMaker")]
    internal sealed class RegPageMakerConfig : RegFactoryConfig<IPageMaker>
    {
        public RegPageMakerConfig()
            : base(PageMakerPlugInFactory.REG_NAME)
        {
        }

        internal RegPageMakerConfig(string regName)
            : this()
        {
            Content = regName;
        }
    }
}
