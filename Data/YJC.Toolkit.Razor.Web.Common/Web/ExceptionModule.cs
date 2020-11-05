using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [Module(Author = "YJC", CreateDate = "2015-08-16", Description = "显示Exception的详细信息")]
    internal class ExceptionModule : BaseCustomModule
    {
        private const string FILE_NAME = "^Normal/Bin/Exception.cshtml";

        public ExceptionModule()
         : base("错误显示")
        {
        }

        public override ISource CreateSource(IPageData pageData)
        {
            return PlugInFactoryManager.CreateInstance<ISource>(
                SourcePlugInFactory.REG_NAME, "ShowException");
        }

        public override IPageMaker CreatePageMaker(IPageData pageData)
        {
            return new FreeRazorPageMaker(FILE_NAME);
        }

        public override bool IsSupportLogOn(IPageData pageData)
        {
            return false;
        }
    }
}