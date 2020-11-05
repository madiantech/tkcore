using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public class CreatorConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_Creator";
        private const string DESCRIPTION = "Creator配置插件工厂";

        public CreatorConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}