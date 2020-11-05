using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    public class RazorTemplateSuiteConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_RazorTemplateSuite";
        private const string DESCRIPTION = "RazorTemplateSuite配置插件工厂";

        public RazorTemplateSuiteConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
