using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public class AutoProcessorConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_AutoProcessor";
        private const string DESCRIPTION = "AutoProcessor配置插件工厂";

        public AutoProcessorConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}