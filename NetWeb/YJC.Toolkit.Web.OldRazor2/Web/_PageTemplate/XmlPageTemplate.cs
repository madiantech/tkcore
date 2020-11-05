using System;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class XmlPageTemplate : IPageTemplate
    {
        private readonly PageTemplateConfigItem fConfig;

        public XmlPageTemplate(PageTemplateConfigItem config)
        {
            fConfig = config;
        }

        #region IPageTemplate 成员

        public string GetTemplateFile(ISource source, IInputData input, OutputData outputData)
        {
            return fConfig.TemplateFile;
        }

        public object GetDefaultPageData(ISource source, IInputData input, OutputData outputData)
        {
            if (fConfig.DefaultPageData != null)
                return fConfig.DefaultPageData.CreateObject();

            return null;
        }

        public Type GetRazorTempate(ISource source, IInputData input, OutputData outputData)
        {
            RazorBaseTemplatePlugInFactory factroy = BaseGlobalVariable.Current.FactoryManager.GetCodeFactory(
                RazorBaseTemplatePlugInFactory.REG_NAME).Convert<RazorBaseTemplatePlugInFactory>();
            return factroy.GetType(fConfig.BaseRazorTemplate);
        }

        #endregion IPageTemplate 成员
    }
}