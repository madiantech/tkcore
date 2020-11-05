using System;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Sys
{
    public class ErrorPageExceptionHandler : IExceptionHandler
    {
        private readonly IPageMaker fPageMaker;

        public ErrorPageExceptionHandler(IPageMaker pageMaker)
        {
            TkDebug.AssertArgumentNull(pageMaker, "pageMaker", null);

            fPageMaker = pageMaker;
        }

        #region IExceptionHandler 成员

        public void HandleException(IWebHandler handler, WebBasePage page, Exception ex)
        {
            TkDebug.AssertArgument(ex is ErrorPageException, "ex", string.Format(ObjectUtil.SysCulture,
                "此Handler只处理ErrorPageException，当前的Exception类型为{0}", ex.GetType()), this);

            ErrorPageSource source = new ErrorPageSource((ErrorPageException)ex);

            InternalWebUtil.WritePage(null, source, fPageMaker, handler);
        }

        #endregion
    }
}
