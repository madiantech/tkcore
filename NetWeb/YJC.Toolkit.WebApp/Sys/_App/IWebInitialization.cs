using System.Web;

namespace YJC.Toolkit.Sys
{
    public interface IWebInitialization : IInitialization
    {
        void SessionStart(HttpApplication application, SessionGlobal sessionGlobal);

        void SessionEnd(HttpApplication application);
    }
}
