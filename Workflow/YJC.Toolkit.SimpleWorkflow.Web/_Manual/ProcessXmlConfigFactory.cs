using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public class ProcessXmlConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_ProcessXml";
        private const string DESCRIPTION = "ProcessXml配置插件工厂";

        public ProcessXmlConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}