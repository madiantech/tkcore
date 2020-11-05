using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class ResolverCreatorConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_ResolverCreator";
        private const string DESCRIPTION = "创建TableResolver的配置插件工厂";

        public ResolverCreatorConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
