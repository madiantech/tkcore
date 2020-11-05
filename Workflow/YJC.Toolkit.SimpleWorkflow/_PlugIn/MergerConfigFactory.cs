using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public class MergerConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_Workflow_Merger";
        private const string DESCRIPTION = "Merger配置插件工厂";

        public MergerConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}