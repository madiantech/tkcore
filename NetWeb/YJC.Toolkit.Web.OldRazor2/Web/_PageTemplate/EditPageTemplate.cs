using System;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [InstancePlugIn]
    [PageTemplate(DefaultModelCreator = "DataSetEdit", Author = "YJC", CreateDate = "2017-04-10",
        Description = "Edit页面模板")]
    internal class EditPageTemplate : IPageTemplate
    {
        public static readonly IPageTemplate Instance = new EditPageTemplate();

        private EditPageTemplate()
        {
        }

        #region IPageTemplate 成员

        public string GetTemplateFile(ISource source, IInputData input, OutputData outputData)
        {
            return @"Normal\Edit\template.cshtml";
        }

        public object GetDefaultPageData(ISource source, IInputData input, OutputData outputData)
        {
            return new NormalEditData();
        }

        public Type GetRazorTempate(ISource source, IInputData input, OutputData outputData)
        {
            return typeof(BaseToolkit2Template);
        }

        #endregion IPageTemplate 成员
    }
}