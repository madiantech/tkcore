using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public class WebPagePlugInFactory : BasePlugInFactory
    {
        public const string REG_NAME = "_tk_WebPage";
        internal const string DESCRIPTION = "Web页面的插件工厂";

        public WebPagePlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
