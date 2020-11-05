using System;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [InstancePlugIn]
    [PageTemplate(DefaultModelCreator = "DataSetMultiEdit", Author = "YJC", CreateDate = "2017-04-16",
        Description = "MultiEdit页面模板")]
    internal class MultiEditPageTemplate : IPageTemplate
    {
        public static readonly IPageTemplate Instance = new MultiEditPageTemplate();

        private MultiEditPageTemplate()
        {
        }

        #region IPageTemplate 成员

        public string GetTemplateFile(ISource source, IInputData input, OutputData outputData)
        {
            return @"Normal\MultiEdit\template.cshtml";
        }

        public object GetDefaultPageData(ISource source, IInputData input, OutputData outputData)
        {
            return new NormalMultiEditData();
        }

        public Type GetRazorTempate(ISource source, IInputData input, OutputData outputData)
        {
            return typeof(BaseMultiEditTemplate);
        }

        #endregion IPageTemplate 成员
    }
}