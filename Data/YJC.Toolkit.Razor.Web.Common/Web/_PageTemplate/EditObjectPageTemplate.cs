using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [InstancePlugIn]
    [PageTemplate(DefaultModelCreator = "ObjectContainerEdit", Author = "YJC", CreateDate = "2019-10-16",
        Description = "Edit Object页面模板")]
    internal class EditObjectPageTemplate : IPageTemplate
    {
        public static readonly IPageTemplate Instance = new EditObjectPageTemplate();

        private EditObjectPageTemplate()
        {
        }

        #region IPageTemplate 成员

        public string GetTemplateFile(ISource source, IInputData input, OutputData outputData)
        {
            return WebRazorUtil.GetTemplateFile("Edit/template.cshtml");
        }

        public object GetDefaultPageData(ISource source, IInputData input, OutputData outputData)
        {
            return new NormalEditData();
        }

        public string GetEngineName(ISource source, IInputData input, OutputData outputData)
        {
            return RazorUtil.TOOLKIT_ENGINE_NAME;
        }

        #endregion IPageTemplate 成员
    }
}