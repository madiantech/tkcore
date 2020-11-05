using System;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Sys
{
    public abstract class BaseOutputExceptionHandler : IExceptionHandler
    {
        protected BaseOutputExceptionHandler()
        {
        }

        #region IExceptionHandler 成员

        public abstract void HandleException(IWebHandler handler, WebBasePage page, Exception ex);

        #endregion

        protected static void HandleException(ISource source,
            IPageMaker pageMaker, IWebHandler handler)
        {
            InternalWebUtil.WritePage(null, source, pageMaker, handler);
        }
    }
}
