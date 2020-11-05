using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    public class EasySearchConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_EasySearch";
        private const string DESCRIPTION = "EasySearch的配置插件工厂";

        public EasySearchConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
