using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Sys
{
    public static class ToolkitExtension
    {
        public static IServiceCollection AddToolkitSvc(this IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddResponseCaching();
            services.AddHostedService<TimedHostedService>();
            services.AddTransient<IUserAgentService, UserAgentService>();
            services.AddTransient<IDeviceService, DeviceService>();
            return services.AddSingleton<IToolkitService, ToolkitService>();
        }

        public static IApplicationBuilder UseToolkit(this IApplicationBuilder app)
        {
            var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            HttpContextHelper.Configure(httpContextAccessor);
            app.UseResponseCaching();

            return app.UseMiddleware<ToolkitMiddleWare>();
        }
    }
}