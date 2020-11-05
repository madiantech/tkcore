using Microsoft.AspNetCore.Http;

namespace YJC.Toolkit.Web
{
    internal class UserAgentService : IUserAgentService
    {
        public UserAgentService(IHttpContextAccessor httpContext)
        {
            var request = httpContext.HttpContext.Request;
            Agent = new UserAgent(request.Headers["User-Agent"]);
        }

        public UserAgent Agent { get; }
    }
}