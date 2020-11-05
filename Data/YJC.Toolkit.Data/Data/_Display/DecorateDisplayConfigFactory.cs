using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class DecorateDisplayConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_DecorateDisplay";
        private const string DESCRIPTION = "装饰Display的配置插件工厂";

        public DecorateDisplayConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
