using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    public class TableSchemeExConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_TableSchemeEx";
        private const string DESCRIPTION = "单表TableSchemeEx配置插件工厂";

        public TableSchemeExConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}