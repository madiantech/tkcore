using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace YJC.Toolkit.Sys
{
    public class ToolkitService : IToolkitService
    {
#if NETCOREAPP2_2
        public ToolkitService(IHostingEnvironment env, IConfiguration configuration,
            ILogger<ToolkitService> logger)
#endif
#if NETCOREAPP3_1

        public ToolkitService(IWebHostEnvironment env, IConfiguration configuration,
            ILogger<ToolkitService> logger)
#endif
        {
            string configXml = configuration.GetSection("Toolkit")?["ApplicationXml"];
            WebApp.ApplicationStart(env, configXml, logger);
        }

        public WebGlobalVariable GlobalVariable => WebGlobalVariable.WebCurrent;

        public WebAppSetting AppSetting => WebAppSetting.WebCurrent;
    }
}