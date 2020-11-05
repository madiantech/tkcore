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
            return WebRazorUtil.GetTemplateFile(fConfig.TemplateFile);
        }

        public object GetDefaultPageData(ISource source, IInputData input, OutputData outputData)
        {
            if (fConfig.DefaultPageData != null)
                return fConfig.DefaultPageData.CreateObject();

            return null;
        }

        public string GetEngineName(ISource source, IInputData input, OutputData outputData)
        {
            return fConfig.RazorEngine;
        }

        #endregion IPageTemplate 成员
    }
}