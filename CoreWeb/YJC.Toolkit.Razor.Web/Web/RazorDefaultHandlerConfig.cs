using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [DefaultHandlerConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2019-10-30",
        Description = "根据配置的Razor文件，显示相应的信息")]
    internal class RazorDefaultHandlerConfig : IDefaultHandler, IConfigCreator<IDefaultHandler>
    {
        private const string DEFAULT_FILE = "^Normal/Bin/Default.cshtml";

        [SimpleAttribute]
        public bool UseTemplate { get; private set; }

        [SimpleAttribute]
        public string FileName { get; private set; }

        private string GetFileName()
        {
            if (string.IsNullOrEmpty(FileName))
                return DEFAULT_FILE;

            return UseTemplate ? WebRazorUtil.GetTemplateFile(FileName) : FileName;
        }

        public IDefaultHandler CreateObject(params object[] args)
        {
            return this;
        }

        public Task Process(HttpContext context)
        {
            dynamic model = new ExpandoObject();
            string fileName = GetFileName();
            IRazorEngine razorEngine = RazorUtil.ToolkitEngine;

            string html = Task.Run(async ()
                => await RazorExtension.CompileRenderWithLayoutAsync(razorEngine,
                fileName, null, model, null)).GetAwaiter().GetResult();
            return context.Response.WriteAsync(html);
        }
    }
}