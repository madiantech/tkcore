using System.Dynamic;
using System.IO;
using System.Threading.Tasks;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal class TkRazorEngine : IRazorEngine
    {
        private readonly IEngineHandler fHandler;

        public TkRazorEngine(IEngineHandler handler)
        {
            TkDebug.AssertArgumentNull(handler, nameof(handler), null);
            fHandler = handler;
            fHandler.RazorEngine = this;
        }

        public TkRazorOptions Options => Handler.Options;

        public IEngineHandler Handler => fHandler;

        public Task<string> CompileRenderAsync<T>(string key, T model, object initData = null,
            ExpandoObject viewBag = null)
        {
            return fHandler.CompileRenderAsync(key, model, initData, viewBag);
        }

        public Task<string> CompileRenderStringAsync<T>(string key, string content, T model,
            object initData = null, ExpandoObject viewBag = null)
        {
            return fHandler.CompileRenderStringAsync(key, content, model, initData, viewBag);
        }

        public Task<ITemplatePage> CompileTemplateAsync(string key)
        {
            return fHandler.CompileTemplateAsync(key);
        }

        public Task RenderTemplateAsync<T>(ITemplatePage templatePage, T model, TextWriter textWriter,
            object initData = null, ExpandoObject viewBag = null)
        {
            return fHandler.RenderTemplateAsync(templatePage, model, textWriter, initData, viewBag);
        }
    }
}