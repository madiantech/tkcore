using System.Web;
using YJC.Toolkit.Cache;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal sealed class ToolkitHandlerFactory : IHttpHandlerFactory
    {
        #region IHttpHandlerFactory 成员

        IHttpHandler IHttpHandlerFactory.GetHandler(HttpContext context, string requestType,
            string url, string pathTranslated)
        {
            ToolkitHttpHandler handler = CacheManager.GetItem("TkHandler",
                pathTranslated).Convert<ToolkitHttpHandler>();
            return handler;
        }

        void IHttpHandlerFactory.ReleaseHandler(IHttpHandler handler)
        {
        }

        #endregion
    }
}
