using System.IO;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class RazorErrorPageExceptionHandler : ErrorPageExceptionHandler
    {
        public RazorErrorPageExceptionHandler()
            : base(CreatePageMaker())
        {
        }

        private static IPageMaker CreatePageMaker()
        {
            TkDebug.ThrowIfNoAppSetting();

            string fileName = Path.Combine(BaseAppSetting.Current.XmlPath,
                @"razortemplate\BootCss\Bin\ErrorPage.cshtml");
            return new FreeRazorPageMaker(fileName);
        }
    }
}
