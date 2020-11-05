using System;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [InstancePlugIn]
    [PageTemplate(Author = "YJC", CreateDate = "2017-04-10", Description = "自定义页面（有预制）模板")]
    internal class CustomPageTemplate : IPageTemplate
    {
        public static readonly IPageTemplate Instance = new CustomPageTemplate();

        private CustomPageTemplate()
        {
        }

        #region IPageTemplate 成员

        public string GetTemplateFile(ISource source, IInputData input, OutputData outputData)
        {
            return @"Normal\Custom\template.cshtml";
        }

        public object GetDefaultPageData(ISource source, IInputData input, OutputData outputData)
        {
            return null;
        }

        public Type GetRazorTempate(ISource source, IInputData input, OutputData outputData)
        {
            return typeof(BaseToolkit2Template);
        }

        #endregion IPageTemplate 成员
    }
}