using System;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Sys
{
    public interface IExceptionHandler
    {
        void HandleException(IWebHandler handler, WebBasePage page, Exception ex);
    }
}
