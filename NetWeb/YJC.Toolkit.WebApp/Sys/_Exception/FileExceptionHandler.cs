using System;
using System.Text;
using YJC.Toolkit.Data;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Sys
{
    public class FileExceptionHandler : IExceptionHandler
    {
        private readonly string fFileName;

        public FileExceptionHandler(string fileName)
        {
            fFileName = fileName;
        }

        #region IExceptionHandler 成员

        public void HandleException(IWebHandler handler, WebBasePage page, Exception ex)
        {
            FileData data = FileData.Create(fFileName);

            handler.Response.ContentEncoding = Encoding.UTF8;
            handler.Response.ContentType = ContentTypeConst.HTML;
            handler.Response.Write(Encoding.UTF8.GetString(data.Data));

            if (LogException)
            {
                string type = page == null ? handler.GetType().ToString() : page.GetType().ToString();
                ExceptionData exData = new ExceptionData(handler.SourceInfo.Source,
                    type, handler.PageUrl.ToString(), ex);
                ExceptionUtil.LogException(exData);
            }
        }

        #endregion

        public bool LogException { get; set; }
    }
}
