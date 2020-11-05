using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [XmlBaseClass(ModuleTemplateConfigItem.BASE_CLASS, typeof(XmlModuleTemplate))]
    [XmlPlugIn(PageTemplatePlugInFactory.PATH, typeof(ModuleTemplateXml), SearchPattern = "*ModuleTemplate.xml")]
    public class ModuleTemplatePlugInFactory : BaseXmlPlugInFactory
    {
        public const string REG_NAME = "_tk_Razor_ModuleTemplate";
        private const string DESCRIPTION = "ModuleTemplate插件工厂";

        public ModuleTemplatePlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}