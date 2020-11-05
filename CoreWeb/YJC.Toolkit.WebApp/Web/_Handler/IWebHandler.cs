using Microsoft.AspNetCore.Http;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public interface IWebHandler : IPageData
    {
        HttpRequest Request { get; }

        HttpResponse Response { get; }

        ISession Session { get; }
    }
}