using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Sys
{
    public class ToolkitMiddleWare
    {
        private readonly RequestDelegate fNext;

        public ToolkitMiddleWare(RequestDelegate next)
        {
            fNext = next;
        }

        public async Task Invoke(HttpContext context, IToolkitService service)
        {
            System.Diagnostics.Trace.WriteLine($"Thread Id  {System.Threading.Thread.CurrentThread.ManagedThreadId}");

            ProcessJWT(context);
            if (WebStartPage.IsDefault(context))
            {
                await WebStartPage.Start(context);
                return;
            }
            PathStringParser parser = new PathStringParser(context.Request.Path);
            if (!string.IsNullOrEmpty(parser.Parser))
            {
                var factory = service.GlobalVariable.FactoryManager.GetCodeFactory(
                    HttpHandlerPlugInFactory.REG_NAME);
                if (factory.Contains(parser.Parser))
                {
                    //LoggerHelper.Logger.LogInformation("Use Parser {0}", parser.Parser);
                    IHttpHandler handler = factory.CreateInstance<IHttpHandler>(parser.Parser);
                    Task task = handler.ProcessRequest(context, fNext, parser);
                    if (task == null)
                        await fNext(context);
                    else
                        await task;
                }
                else
                    await fNext(context);
            }
            else
                await fNext(context);
        }

        private void ProcessJWT(HttpContext context)
        {
            var request = context.Request;
            string auth = request.Cookies[JWTUtil.COOKIE_NAME];
            if (!string.IsNullOrEmpty(auth))
            {
                try
                {
                    var info = JWTUtil.DecodeFromJwt(auth);
                    if (JWTUtil.IsValidHost(info, request.Host.Host, request.Host.Port))
                        context.User = new ToolkitClaimsPrincipal(info);
                }
                catch (Exception ex)
                {
                    TkTrace.LogError(ex.Message);
                }
            }
        }
    }
}