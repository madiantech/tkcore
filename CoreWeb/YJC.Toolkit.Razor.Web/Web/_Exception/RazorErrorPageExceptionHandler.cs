using System.IO;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class RazorErrorPageExceptionHandler : ErrorPageExceptionHandler
    {
        public RazorErrorPageExceptionHandler(bool useTemplate)
            : base(CreatePageMaker(useTemplate))
        {
        }

        private static IPageMaker CreatePageMaker(bool useTemplate)
        {
            ErrorPageRazorPageMakerConfig config = new ErrorPageRazorPageMakerConfig
            {
                UseTemplate = useTemplate
            };
            return config.CreateObject();
        }
    }
}