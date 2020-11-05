using System;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [InstancePlugIn]
    [PageTemplate(DefaultModelCreator = "DataSetList", Author = "YJC", CreateDate = "2019-04-29",
        Description = "支持统计的List页面模板")]
    internal class StatListPageTemplate : IPageTemplate
    {
        public static readonly IPageTemplate Instance = new StatListPageTemplate();

        private StatListPageTemplate()
        {
        }

        #region IPageTemplate 成员

        public string GetTemplateFile(ISource source, IInputData input, OutputData outputData)
        {
            if (input.QueryString["GetData"] == "Page" || input.IsPost)
                return @"Normal\StatList\listmain.cshtml";
            else
                return @"Normal\StatList\template.cshtml";
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