using System.Dynamic;
using System.IO;
using System.Threading.Tasks;

namespace YJC.Toolkit.Razor
{
    public interface IRazorEngine
    {
        TkRazorOptions Options { get; }

        IEngineHandler Handler { get; }

        Task<string> CompileRenderAsync<T>(string key, T model, object initData = null, ExpandoObject viewBag = null);

        Task<string> CompileRenderStringAsync<T>(string key, string content, T model, object initData = null, ExpandoObject viewBag = null);

        Task<ITemplatePage> CompileTemplateAsync(string key);

        Task RenderTemplateAsync<T>(ITemplatePage templatePage, T model, TextWriter textWriter, object initData = null, ExpandoObject viewBag = null);
    }
}