using System.Web;
using Microsoft.AspNetCore.Hosting;

namespace YJC.Toolkit.Sys
{
    public interface IWebInitialization : IInitialization
    {
#if NETCOREAPP2_2
       void SessionStart(IHostingEnvironment application, SessionGlobal sessionGlobal);

       void SessionEnd(IHostingEnvironment application);
#endif

#if NETCOREAPP3_1

        void SessionStart(IWebHostEnvironment application, SessionGlobal sessionGlobal);

        void SessionEnd(IWebHostEnvironment application);

#endif
    }
}