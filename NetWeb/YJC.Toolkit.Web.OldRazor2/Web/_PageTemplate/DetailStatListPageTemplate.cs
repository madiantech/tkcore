using System;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [InstancePlugIn]
    [PageTemplate(DefaultModelCreator = "DataSetList", Author = "YJC", CreateDate = "2019-05-06",
        Description = "Detail StatList页面模板")]
    internal class DetailStatListPageTemplate : IPageTemplate
    {
        public static readonly IPageTemplate Instance = new DetailStatListPageTemplate();

        private DetailStatListPageTemplate()
        {
        }

        #region IPageTemplate 成员

        public string GetTemplateFile(ISource source, IInputData input, OutputData outputData)
        {
            return @"Normal\DetailStatList\template.cshtml";
        }

        public object GetDefaultPageData(ISource source, IInputData input, OutputData outputData)
        {
            return new NormalStatListData();
        }

        public Type GetRazorTempate(ISource source, IInputData input, OutputData outputData)
        {
            return typeof(BaseListTemplate);
        }

        #endregion IPageTemplate 成员
    }
}