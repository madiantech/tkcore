using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Sys
{
    public class DefaultHandlerConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_DefaultHandler";
        private const string DESCRIPTION = "DefaultHandler配置插件工厂";

        public DefaultHandlerConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
