using System;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Razor
{
    internal static class RazorPageTemplateUtil
    {
        private static string Execute(object model, DynamicObjectBag viewBag, PageTemplateInitData initData,
            IPageTemplate pageTemplate, ISource source, IInputData input, OutputData outputData)
        {
            Type baseType = pageTemplate.GetRazorTempate(source, input, outputData);
            string content = RazorUtil.ParseFromFile(baseType, initData.FileName, initData.LayoutFile,
                null, model, viewBag, initData);

            return content;
        }

        public static IPageTemplate CreatePageTemplate(string regName)
        {
            TkDebug.AssertArgumentNullOrEmpty(regName, "regName", null);

            return PlugInFactoryManager.CreateInstance<IPageTemplate>(PageTemplatePlugInFactory.REG_NAME, regName);
        }

        public static string Execute(IPageTemplate template, string pageTemplateName, string modelCreator,
            string localRazorFile, object model, DynamicObjectBag viewBag, ISource source,
            IInputData input, OutputData outputData)
        {
            string templateFile = template.GetTemplateFile(source, input, outputData);
            PageTemplateInitData initData = new PageTemplateInitData(templateFile, localRazorFile,
                pageTemplateName, modelCreator);
            return Execute(model, viewBag, initData, template, source, input, outputData);
        }
    }
}