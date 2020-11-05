using System.IO;
using System.Web;

namespace YJC.Toolkit.Web
{
    class ToolkitAbbreviateHandlerFactory : IHttpHandlerFactory
    {
        #region IHttpHandlerFactory 成员

        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            string abbr = Path.GetFileNameWithoutExtension(url);
            ToolkitAbbreviateHandler handler = new ToolkitAbbreviateHandler(abbr);
            return handler;
        }

        public void ReleaseHandler(IHttpHandler handler)
        {
        }

        #endregion
    }
}
