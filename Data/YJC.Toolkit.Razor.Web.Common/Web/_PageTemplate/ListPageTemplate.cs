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
                return WebRazorUtil.GetTemplateFile("List/listmain.cshtml");
            else
                return WebRazorUtil.GetTemplateFile("List/template.cshtml");
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