using System;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [InstancePlugIn]
    [PageTemplate(Author = "YJC", CreateDate = "2017-04-16", Description = "Tree页面模板")]
    internal class TreePageTemplate : IPageTemplate
    {
        public static readonly IPageTemplate Instance = new TreePageTemplate();

        private TreePageTemplate()
        {
        }

        #region IPageTemplate 成员

        public string GetTemplateFile(ISource source, IInputData input, OutputData outputData)
        {
            return @"Normal\Tree\template.cshtml";
        }

        public object GetDefaultPageData(ISource source, IInputData input, OutputData outputData)
        {
            return new NormalTreeData();
        }

        public Type GetRazorTempate(ISource source, IInputData input, OutputData outputData)
        {
            return typeof(BaseToolkit2Template);
        }

        #endregion IPageTemplate 成员
    }
}