using System.Web;
using System.Web.SessionState;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public interface IWebHandler : IPageData
    {
        HttpRequest Request { get; }

        HttpResponse Response { get; }

        HttpSessionState Session { get; }
    }
}
