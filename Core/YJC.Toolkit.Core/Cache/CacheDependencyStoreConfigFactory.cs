using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    public class CacheDependencyStoreConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_CacheDependencyStore";
        private const string DESCRIPTION = "CacheDependencyStore配置插件工厂";

        public CacheDependencyStoreConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
