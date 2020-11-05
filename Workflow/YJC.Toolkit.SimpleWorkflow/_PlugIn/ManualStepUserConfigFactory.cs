using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public class ManualStepUserConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_ManualStepUser";
        private const string DESCRIPTION = "ManualStepUser配置插件工厂";

        public ManualStepUserConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}