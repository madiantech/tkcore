using System;
using System.Collections.Generic;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [XmlBaseClass(PageTemplateConfigItem.BASE_CLASS, typeof(XmlPageTemplate))]
    [XmlPlugIn(PATH, typeof(PageTemplateXml), SearchPattern = "*PageTemplate.xml")]
    public class PageTemplatePlugInFactory : BaseXmlPlugInFactory
    {
        public const string REG_NAME = "_tk_Razor_PageTemplate";
        public const string PATH = "RazorConfigTemplate";
        private const string DESCRIPTION = "PageTemplate插件工厂";
        private readonly Dictionary<string, string> fDefault;

        public PageTemplatePlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
            fDefault = new Dictionary<string, string>();
        }

        protected override bool Add(string regName, BasePlugInAttribute attribute, Type type)
        {
            bool result = base.Add(regName, attribute, type);
            if ((result))
            {
                PageTemplateAttribute attr = attribute.Convert<PageTemplateAttribute>();
                if (!string.IsNullOrEmpty(attr.DefaultModelCreator))
                    fDefault.Add(regName, attr.DefaultModelCreator);
            }
            return result;
        }

        protected override void Add(string regName, IXmlPlugInItem plugItem, Type baseType)
        {
            base.Add(regName, plugItem, baseType);

            PageTemplateConfigItem config = plugItem.Convert<PageTemplateConfigItem>();
            if (!string.IsNullOrEmpty(config.DefaultModelCreator))
                fDefault.Add(regName, config.DefaultModelCreator);
        }

        private IModelCreator InternalCreateModelCreator(string pageTemplate)
        {
            string regName;
            if (fDefault.TryGetValue(pageTemplate, out regName))
                return PlugInFactoryManager.CreateInstance<IModelCreator>(
                    ModelCreatorPlugInFactory.REG_NAME, regName);
            return null;
        }

        public static IModelCreator CreateModelCreator(string pageTemplate)
        {
            TkDebug.AssertArgumentNullOrEmpty(pageTemplate, "pageTemplate", null);

            TkDebug.ThrowIfNoGlobalVariable();
            PageTemplatePlugInFactory factory = BaseGlobalVariable.Current.FactoryManager
                .GetCodeFactory(REG_NAME).Convert<PageTemplatePlugInFactory>();
            IModelCreator creator = factory.InternalCreateModelCreator(pageTemplate);

            return creator;
        }
    }
}