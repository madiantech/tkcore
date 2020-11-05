using System.IO;
using YJC.Toolkit.Data;
using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [Module(Author = "YJC", CreateDate = "2015-08-16", Description = "显示Exception的详细信息")]
    internal class ExceptionModule : BaseCustomModule
    {
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
            string fileName = Path.Combine(BaseAppSetting.Current.XmlPath,
                @"razortemplate\BootCss\Bin\Exception.cshtml");
            return new FreeRazorPageMaker(fileName);
        }
    }
}