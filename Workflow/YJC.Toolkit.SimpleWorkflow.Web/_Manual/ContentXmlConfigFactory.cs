using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public class ContentXmlConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_ContentXml";
        private const string DESCRIPTION = "ContentXml配置插件工厂";

        public ContentXmlConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}