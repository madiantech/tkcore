using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [InstancePlugIn]
    [PageTemplate(DefaultModelCreator = "DataSetList", Author = "YJC", CreateDate = "2017-04-16",
        Description = "Detail List页面模板")]
    internal class DetailListPageTemplate : IPageTemplate
    {
        public static readonly IPageTemplate Instance = new DetailListPageTemplate();

        private DetailListPageTemplate()
        {
        }

        #region IPageTemplate 成员

        public string GetTemplateFile(ISource source, IInputData input, OutputData outputData)
        {
            return WebRazorUtil.GetTemplateFile("DetailList/template.cshtml");
        }

        public object GetDefaultPageData(ISource source, IInputData input, OutputData outputData)
        {
            return new NormalListData();
        }

        public string GetEngineName(ISource source, IInputData input, OutputData outputData)
        {
            return RazorUtil.LIST_ENGINE_NAME;
        }

        #endregion IPageTemplate 成员
    }
}