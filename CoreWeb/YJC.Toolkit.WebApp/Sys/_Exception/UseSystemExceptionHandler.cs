using System;
using System.Threading.Tasks;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Sys
{
    public sealed class UseSystemExceptionHandler : IExceptionHandler
    {
        public static readonly IExceptionHandler Handler = new UseSystemExceptionHandler();

        private UseSystemExceptionHandler()
        {
        }

        public Task HandleException(IWebHandler handler, WebBasePage page, Exception ex)
        {
            throw new Exception("空Exception，具体请看InnerException", ex);
        }
    }
}