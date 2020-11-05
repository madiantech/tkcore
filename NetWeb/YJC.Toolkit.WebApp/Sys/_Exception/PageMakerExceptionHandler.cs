using System;
using YJC.Toolkit.Data;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Sys
{
    public class PageMakerExceptionHandler : IExceptionHandler
    {
        private readonly IPageMaker fPageMaker;

        public PageMakerExceptionHandler(IPageMaker pageMaker)
        {
            TkDebug.AssertArgumentNull(pageMaker, "pageMaker", null);

            fPageMaker = pageMaker;
        }

        #region IExceptionHandler 成员

        public void HandleException(IWebHandler handler, WebBasePage page, Exception ex)
        {
            string type = page == null ? handler.GetType().ToString() : page.GetType().ToString();

            ExceptionSource source = new ExceptionSource(handler.SourceInfo.Source,
                type, handler.PageUrl.ToString(), ex);
            if (LogException || handler.IsPost)
            {
                string fileName = ExceptionUtil.LogException(source.Data);
                source.FileName = fileName;
            }

            InternalWebUtil.WritePage(null, source, fPageMaker, handler);
        }

        #endregion

        public bool LogException { get; set; }
    }
}
