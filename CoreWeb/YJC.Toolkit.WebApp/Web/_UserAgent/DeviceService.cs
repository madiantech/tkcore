using Microsoft.AspNetCore.Http;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class DeviceService : IDeviceService
    {
        public DeviceService(IHttpContextAccessor httpContext)
        {
            var request = httpContext.HttpContext.Request;
            Device = WebAgentUtil.JudgeClient(request.Headers["User-Agent"]);
        }

        public DeviceType Device { get; }
    }
}