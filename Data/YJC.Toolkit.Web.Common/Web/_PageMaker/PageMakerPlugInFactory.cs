using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public class PageMakerPlugInFactory : BaseInstancePlugInFactory
    {
        public const string REG_NAME = "_tk_PageMaker";
        private const string DESCRIPTION = "生成Web页面的插件工厂";

        public PageMakerPlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }

    }
}
