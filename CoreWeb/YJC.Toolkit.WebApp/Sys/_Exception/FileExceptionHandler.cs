using Microsoft.AspNetCore.Http;
using System;
using System.Text;
using System.Threading.Tasks;
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

        public Task HandleException(IWebHandler handler, WebBasePage page, Exception ex)
        {
            FileData data = FileData.Create(fFileName);

            if (LogException)
            {
                string type = page == null ? handler.GetType().ToString() : page.GetType().ToString();
                ExceptionData exData = new ExceptionData(handler.SourceInfo.Source,
                    type, handler.PageUrl.ToString(), ex);
                ExceptionUtil.LogException(exData);
            }
            handler.Response.ContentType = ContentTypeConst.HTML;
            return handler.Response.WriteAsync(Encoding.UTF8.GetString(data.Data), Encoding.UTF8);
        }

        #endregion IExceptionHandler 成员

        public bool LogException { get; set; }
    }
}