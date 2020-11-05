using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public class ProcessorConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_Processor";
        private const string DESCRIPTION = "Processor配置插件工厂";

        public ProcessorConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}