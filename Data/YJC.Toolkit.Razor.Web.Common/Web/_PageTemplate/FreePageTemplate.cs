using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [InstancePlugIn]
    [PageTemplate(Author = "YJC", CreateDate = "2017-04-10", Description = "自由页面（无任何预制模板）模板")]
    internal class FreePageTemplate : IPageTemplate
    {
        public static readonly IPageTemplate Instance = new FreePageTemplate();

        private FreePageTemplate()
        {
        }

        #region IPageTemplate 成员

        public string GetTemplateFile(ISource source, IInputData input, OutputData outputData)
        {
            return null;
        }

        public object GetDefaultPageData(ISource source, IInputData input, OutputData outputData)
        {
            return null;
        }

        public string GetEngineName(ISource source, IInputData input, OutputData outputData)
        {
            return RazorUtil.TOOLKIT_ENGINE_NAME;
        }

        #endregion IPageTemplate 成员
    }
}