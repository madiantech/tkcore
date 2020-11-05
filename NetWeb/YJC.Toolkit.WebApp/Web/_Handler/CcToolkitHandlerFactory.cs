using System.Web;
using YJC.Toolkit.Web.Page;

namespace YJC.Toolkit.Web
{
    internal sealed class CcToolkitHandlerFactory : IHttpHandlerFactory
    {
        #region IHttpHandlerFactory 成员

        public IHttpHandler GetHandler(HttpContext context, string requestType,
            string url, string pathTranslated)
        {
            UrlParser parser = UrlParser.Create(url);
            var info = parser.Info;
            if (info.IsContent)
                return new WebModuleContentHttpHandler()
                {
                    SourceInfo = info
                };
            else
                return new WebModuleRedirectHttpHandler()
                {
                    SourceInfo = info
                };
        }

        public void ReleaseHandler(IHttpHandler handler)
        {
        }

        #endregion
    }
}
