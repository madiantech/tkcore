using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace YJC.Toolkit.Sys
{
    [DefaultHandlerConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2019-10-29",
        Description = "空的DefaultHandler，不处理任何请求")]
    internal class EmptyDefaultHandlerConfig : IDefaultHandler, IConfigCreator<IDefaultHandler>
    {
        public static readonly IDefaultHandler Hanlder = new EmptyDefaultHandlerConfig();

        public IDefaultHandler CreateObject(params object[] args)
        {
            return this;
        }

        public Task Process(HttpContext context)
        {
            return Task.CompletedTask;
        }
    }
}