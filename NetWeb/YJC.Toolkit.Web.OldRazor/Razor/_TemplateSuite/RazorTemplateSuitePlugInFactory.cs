using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    [XmlBaseClass(StandardConfig.BASE_CLASS, typeof(StandardRazorTemplateSuite))]
    [XmlPlugIn(PATH, typeof(RazorTemplateXml), SearchPattern = "*.xml")]
    public class RazorTemplateSuitePlugInFactory : BaseXmlPlugInFactory
    {
        public const string REG_NAME = "_tk_RazorTemplateSuite";
        public const string PATH = "razortemplatesuite";
        private const string DESCRIPTION = "RazorTemplateSuite插件工厂";

        public RazorTemplateSuitePlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
