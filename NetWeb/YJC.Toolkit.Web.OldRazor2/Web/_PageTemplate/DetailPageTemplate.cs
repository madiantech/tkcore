using System;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [InstancePlugIn]
    [PageTemplate(DefaultModelCreator = "DataSetDetail", Author = "YJC", CreateDate = "2017-04-10",
        Description = "Detail页面模板")]
    internal class DetailPageTemplate : IPageTemplate
    {
        public static readonly IPageTemplate Instance = new DetailPageTemplate();

        private DetailPageTemplate()
        {
        }

        #region IPageTemplate 成员

        public string GetTemplateFile(ISource source, IInputData input, OutputData outputData)
        {
            return @"Normal\Detail\template.cshtml";
        }

        public object GetDefaultPageData(ISource source, IInputData input, OutputData outputData)
        {
            return new NormalDetailData();
        }

        public Type GetRazorTempate(ISource source, IInputData input, OutputData outputData)
        {
            return typeof(BaseToolkit2Template);
        }

        #endregion IPageTemplate 成员
    }
}