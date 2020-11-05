using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    public class TableSchemeConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_TableScheme";
        private const string DESCRIPTION = "单表Scheme的配置插件工厂";

        public TableSchemeConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
