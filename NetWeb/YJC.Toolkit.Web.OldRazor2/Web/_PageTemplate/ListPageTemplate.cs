using System;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [InstancePlugIn]
    [PageTemplate(DefaultModelCreator = "DataSetList", Author = "YJC", CreateDate = "2017-04-10",
        Description = "List页面模板")]
    internal class ListPageTemplate : IPageTemplate
    {
        public static readonly IPageTemplate Instance = new ListPageTemplate();

        private ListPageTemplate()
        {
        }

        #region IPageTemplate 成员

        public string GetTemplateFile(ISource source, IInputData input, OutputData outputData)
        {
            if (input.QueryString["GetData"] == "Page" || input.IsPost)
                return @"Normal\List\listmain.cshtml";
            else
                return @"Normal\List\template.cshtml";
        }

        public object GetDefaultPageData(ISource source, IInputData input, OutputData outputData)
        {
            return new NormalListData();
        }

        public Type GetRazorTempate(ISource source, IInputData input, OutputData outputData)
        {
            return typeof(BaseListTemplate);
        }

        #endregion IPageTemplate 成员
    }
}