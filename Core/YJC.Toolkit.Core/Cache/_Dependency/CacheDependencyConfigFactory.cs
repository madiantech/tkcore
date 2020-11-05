using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    public sealed class CacheDependencyConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_CacheDependency";
        private const string DESCRIPTION = "缓存依赖的配置插件工厂";

        public CacheDependencyConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
