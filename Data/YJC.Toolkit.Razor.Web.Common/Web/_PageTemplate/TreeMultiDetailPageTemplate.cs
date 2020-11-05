using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [InstancePlugIn]
    [PageTemplate(DefaultModelCreator = "DataSetDetail", Author = "YJC", CreateDate = "2017-04-16",
        Description = "Tree MultiDetail页面模板")]
    internal class TreeMultiDetailPageTemplate : IPageTemplate
    {
        public static readonly IPageTemplate Instance = new TreeMultiDetailPageTemplate();

        private TreeMultiDetailPageTemplate()
        {
        }

        #region IPageTemplate 成员

        public string GetTemplateFile(ISource source, IInputData input, OutputData outputData)
        {
            return WebRazorUtil.GetTemplateFile("Tree/multidetailtemplate.cshtml");
        }

        public object GetDefaultPageData(ISource source, IInputData input, OutputData outputData)
        {
            return new NormalMultiDetailData();
        }

        public string GetEngineName(ISource source, IInputData input, OutputData outputData)
        {
            return RazorUtil.TOOLKIT_ENGINE_NAME;
        }

        #endregion IPageTemplate 成员
    }
}