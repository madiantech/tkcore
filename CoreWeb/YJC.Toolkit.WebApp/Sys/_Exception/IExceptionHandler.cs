using System;
using System.Threading.Tasks;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Sys
{
    public interface IExceptionHandler
    {
        Task HandleException(IWebHandler handler, WebBasePage page, Exception ex);
    }
}