using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class CoreDisplayConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_Display";
        private const string DESCRIPTION = "字段Display的配置插件工厂";

        public CoreDisplayConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
