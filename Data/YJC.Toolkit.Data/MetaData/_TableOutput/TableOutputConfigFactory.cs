using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    public class TableOutputConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_TableOutput";
        private const string DESCRIPTION = "TableOutput配置插件工厂";

        public TableOutputConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}